

using UnityEngine;
using UnityEngine.EventSystems;

namespace Chessticle
{
    public class PieceUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        void Awake()
        {
            if (s_ChessboardUI == null)
            {
                s_ChessboardUI = GetComponentInParent<ChessboardUI>();
            }

            m_RectTransform = GetComponent<RectTransform>();
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            transform.SetAsLastSibling(); // move to front
            s_ChessboardUI.OnStartMove(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            s_ChessboardUI.OnEndMove(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            m_RectTransform.position = eventData.position;
        }

        RectTransform m_RectTransform;
        static ChessboardUI s_ChessboardUI;
    }
}