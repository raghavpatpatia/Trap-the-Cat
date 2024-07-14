using System;
using System.Collections;
using UnityEngine;

public class TileController : IDisposable
{
    public TileModel TileModel { get; private set; }
    public TileView TileView { get; private set; }
    private CommandInvoker commandInvoker;
    private EventService eventService;
    private Coroutine OnTileClickCoroutine;
    public TileController(TileSO tileSO, Vector3 position, Transform parentTransform, CommandInvoker commandInvoker, Vector2Int gridPosition, EventService eventService) 
    {
        this.eventService = eventService;
        this.TileModel = new TileModel(tileSO, gridPosition);
        this.TileView = GameObject.Instantiate<TileView>(tileSO.TileView, position, Quaternion.identity, parentTransform);
        this.TileView.Init(this, eventService);
        this.commandInvoker = commandInvoker;
        SetTile();
        SubscribeEvent();
    }
    
    private void SubscribeEvent()
    {
        eventService.OnTileClick.AddListener(OnTileClick);
    }

    private void SetTile()
    {
        this.TileModel.SetTileState(TileState.EMPTY);
        this.TileView.ChangeSpriteColor(TileModel.EmptyTileColor);
    }

    public void OnTileClick(TileController tileController)
    {
        if (OnTileClickCoroutine != null)
        {
            TileView.StopCoroutine(OnTileClickCoroutine);
        }
        OnTileClickCoroutine = TileView.StartCoroutine(OnTileClickEnumerator(tileController));
    }

    private IEnumerator OnTileClickEnumerator(TileController tileController)
    {
        TileUnit tileClickCommand = new TileClickCommand(tileController);
        commandInvoker.ProcessCommand(tileClickCommand as ICommand);

        yield return new WaitForSeconds(2f);
    }

    public Vector2 GetTileCenter() => TileView.GetTileCenter();

    public void Dispose()
    {
        eventService.OnTileClick.RemoveListener(OnTileClick);
    }
}