try {
    document.documentElement.dataset.theme = localStorage.getItem('observer-theme') ?? 'system';
} catch {
}
