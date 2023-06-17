

using UnityEngine;
using Object = UnityEngine.Object;

namespace Chessticle
{
    // A pool of SVG images for a piece
    public class PieceImagePool
    {
        public PieceImagePool(Unity.VectorGraphics.SVGImage template, Sprite sprite, Piece piece)
        {
            int maxCount = 0;
            // 8 pawns can be promoted to a rook, queen, bishop or knight
            // so there can be up to (2 + 8) rooks, (1 + 8) queens etc.
            switch (piece)
            {
                case Piece.WhitePawn:
                case Piece.BlackPawn:
                    maxCount = 8;
                    break;
                case Piece.Knight:
                    maxCount = 10;
                    break;
                case Piece.King:
                    maxCount = 1;
                    break;
                case Piece.Bishop:
                    maxCount = 10;
                    break;
                case Piece.Rook:
                    maxCount = 10;
                    break;
                case Piece.Queen:
                    maxCount = 9;
                    break;
            } 
        
            m_SvgImages = new Unity.VectorGraphics.SVGImage[maxCount];
        
            for (int i = 0; i < maxCount; i++)
            {
                var go = Object.Instantiate(template, template.transform.parent);
                var pieceImage = go.GetComponent<Unity.VectorGraphics.SVGImage>();
                pieceImage.sprite = sprite;
                pieceImage.name = sprite.name;
                m_SvgImages[i] = pieceImage;
            }
        }

        public Unity.VectorGraphics.SVGImage GetImage()
        {
            m_Idx = (m_Idx + 1) % m_SvgImages.Length;
            m_SvgImages[m_Idx].enabled = true;
            return m_SvgImages[m_Idx];
        }

        public void HideAll()
        {
            foreach (var image in m_SvgImages)
            {
                image.enabled = false;
            }
        }

        public void SetDraggingEnabled(bool enabled)
        {
            foreach (var image in m_SvgImages)
            {
                image.raycastTarget = enabled;
            }
        }

        readonly Unity.VectorGraphics.SVGImage[] m_SvgImages;
        int m_Idx;
    }
}