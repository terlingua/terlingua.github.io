const DB_NAME = 'terlingua-books';
const DB_VERSION = 1;
const STORE_NAME = 'progress';

let dbPromise = null;

function openDatabase() {
    dbPromise ??= new Promise((resolve, reject) => {
        const request = indexedDB.open(DB_NAME, DB_VERSION);

        request.onupgradeneeded = () => {
            const db = request.result;
            if (!db.objectStoreNames.contains(STORE_NAME)) {
                db.createObjectStore(STORE_NAME);
            }
        };

        request.onsuccess = () => {
            const db = request.result;
            db.onversionchange = db.onclose = () => {
                db.close();
                dbPromise = null;
            };
            resolve(db);
        };

        request.onerror = () => reject(request.error);
        request.onblocked = () => reject(new Error('IndexedDB upgrade is blocked by another tab'));
    });

    dbPromise.catch(() => {
        dbPromise = null;
    });

    return dbPromise;
}

async function withStore(mode, operation) {
    const db = await openDatabase();
    return new Promise((resolve, reject) => {
        const transaction = db.transaction(STORE_NAME, mode);
        const request = operation(transaction.objectStore(STORE_NAME));
        transaction.oncomplete = () => resolve(request.result);
        transaction.onerror = () => reject(transaction.error);
        transaction.onabort = () => reject(transaction.error ?? new Error('IndexedDB transaction aborted'));
    });
}

export async function get(key) {
    const value = await withStore('readonly', store => store.get(key));
    return typeof value === 'string' ? value : null;
}

export async function set(key, value) {
    await withStore('readwrite', store => store.put(value, key));
}

export async function remove(key) {
    await withStore('readwrite', store => store.delete(key));
}

export async function isPersistent() {
    try {
        await openDatabase();
        return true;
    } catch {
        return false;
    }
}
