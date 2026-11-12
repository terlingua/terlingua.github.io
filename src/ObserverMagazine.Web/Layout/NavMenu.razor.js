const STORAGE_KEY = 'observer-theme';

export function getTheme() {
    return document.documentElement.dataset.theme ?? 'system';
}

export function setTheme(theme) {
    document.documentElement.dataset.theme = theme;
    try {
        localStorage.setItem(STORAGE_KEY, theme);
    } catch {
    }
}
