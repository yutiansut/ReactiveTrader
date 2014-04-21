class Profiler implements IProfiler {
    private _now: () => number;

    constructor() {
        if (typeof window.performance != "undefined") {
            this._now = window.performance.now;
        }
        else {
            this._now = () => new Date().getTime();
        }
    }

    now(): number {
        return this._now();
    }
} 