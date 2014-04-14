interface ISpotTileViewModel extends  Rx.IDisposable
{
    pricing: IPricingViewModel;
    affirmation: IAffirmationViewModel;
    error: IErrorViewModel;
    config: IConfigViewModel;
    state: TileState;
    currencyPair: string;
    onTrade(trade: ITrade): void;
    onExecutionError(message: string);
    toConfig(): void;
    dismissAffirmation(): void;
    dismissError(): void;
} 