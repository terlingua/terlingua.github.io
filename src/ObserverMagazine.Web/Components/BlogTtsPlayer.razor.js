const STORAGE_KEY = 'observer-audio-speed';

export function getSavedRate() {
    try {
        const rate = Number.parseFloat(localStorage.getItem(STORAGE_KEY));
        return Number.isFinite(rate) && rate >= 0.5 && rate <= 3 ? rate : 1;
    } catch {
        return 1;
    }
}

export function applyRate(audio, rate) {
    audio.defaultPlaybackRate = rate;
    audio.playbackRate = rate;
}

export function saveRate(rate) {
    try {
        localStorage.setItem(STORAGE_KEY, String(rate));
    } catch {
    }
}
