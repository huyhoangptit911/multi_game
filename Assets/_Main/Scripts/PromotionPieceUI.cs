

using UnityEngine;
using UnityEngine.EventSystems;

namespace Chessticle
{
    public class PromotionPieceUI: MonoBehaviour, IPointerClickHandler
    {
        public Piece Piece;
        
        void Awake()
        {
            if (s_ChessboardUI == null)
            {
                s_ChessboardUI = GetComponentInParent<ChessboardUI>();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            s_ChessboardUI.OnPromotionPieceSelected(Piece);
        }

        static ChessboardUI s_ChessboardUI;
    }
}