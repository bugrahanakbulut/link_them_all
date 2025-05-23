using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using LinkThemAll.Game.Board;
using LinkThemAll.Game.Tile;
using LinkThemAll.Services.Task;

namespace LinkThemAll.Game.Tasks
{
    public class LinkTilesTask : IServiceTask
    {
        private readonly BoardController _boardController;
        private readonly IReadOnlyList<BoardTile> _linkedTiles;
        private readonly BoardTilePool _boardTilePool;
        
        private const float TILE_FADE_OUT_DURATION = 0.36f;
        
        public LinkTilesTask(
            BoardController boardController, 
            IReadOnlyList<BoardTile> linkedTiles,
            BoardTilePool pool)
        {
            _boardController = boardController;
            _linkedTiles = linkedTiles;
            _boardTilePool = pool;
        }

        public async UniTask Execute()
        {
            Sequence sequence = DOTween.Sequence();

            _boardController.TilesLinked(_linkedTiles);
            
            foreach (BoardTile tile in _linkedTiles)
            {
                sequence.Join(tile.FadeOut(TILE_FADE_OUT_DURATION));
            }

            sequence.OnComplete(TilesLinked);
            
            sequence.SetId(_boardController);

            await sequence;
        }

        private void TilesLinked()
        {
            foreach (BoardTile tile in _linkedTiles)
            {
                _boardTilePool.Release(tile);
            }
        }
    }
}