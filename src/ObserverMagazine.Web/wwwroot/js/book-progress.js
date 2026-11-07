const DB_NAME = 'terlingua-books';
const DB_VERSION = 1;
const STORE_NAME = 'progress';
const OPEN_TIMEOUT_MS = 3000;

let dbPromise = null;

function openDatabase() {
    if (dbPromise) {
        return dbPromise;
    }

    dbPromise = new Promise((resolve, reject) => {
        if (typeof indexedDB === 'undefined' || indexedDB === null) {
            reject(new Error('IndexedDB is not available'));
            return;
        }

        let settled = false;
        const timer = setTimeout(() => {
            settled = true;
            reject(new Error('Timed out opening IndexedDB'));
        }, OPEN_TIMEOUT_MS);

        let request;
        try {
            request = indexedDB.open(DB_NAME, DB_VERSION);
        } catch (error) {
            clearTimeout(timer);
            reject(error);
            return;
        }

        request.onupgradeneeded = () => {
            const db = request.result;
            if (!db.objectStoreNames.contains(STORE_NAME)) {
                db.createObjectStore(STORE_NAME);
            }
        };

        request.onsuccess = () => {
            const db = request.result;
            clearTimeout(timer);
            if (settled) {
                db.close();
                return;
            }
            settled = true;
            db.onversionchange = () => {
                db.close();
                dbPromise = null;
            };
            db.onclose = () => {
                dbPromise = null;
            };
            resolve(db);
        };

        request.onerror = () => {
            clearTimeout(timer);
            if (settled) {
                return;
            }
            settled = true;
            reject(request.error || new Error('Failed to open IndexedDB'));
        };
    });

    dbPromise.catch(() => {
        dbPromise = null;
    });

    return dbPromise;
}

function withStore(mode, operation) {
    return openDatabase().then((db) => new Promise((resolve, reject) => {
        const transaction = db.transaction(STORE_NAME, mode);
        const request = operation(transaction.objectStore(STORE_NAME));
        let result;

        request.onsuccess = () => {
            result = request.result;
        };
        transaction.oncomplete = () => resolve(result);
        transaction.onerror = () => reject(transaction.error || new Error('IndexedDB transaction failed'));
        transaction.onabort = () => reject(transaction.error || new Error('IndexedDB transaction aborted'));
    }));
}

export function get(key) {
    return withStore('readonly', (store) => store.get(key))
        .then((value) => (typeof value === 'string' ? value : null));
}

export function set(key, value) {
    return withStore('readwrite', (store) => store.put(value, key)).then(() => undefined);
}

export function remove(key) {
    return withStore('readwrite', (store) => store.delete(key)).then(() => undefined);
}

export function isPersistent() {
    return openDatabase().then(() => true, () => false);
}
