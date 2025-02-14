using UnityEngine;

namespace TarodevController
{
    public partial class PlayerController
    {
        private bool _isSwimming;
        
        private void CalculateSwimming()
        {
            if (!Stats.AllowSwimming) return;
            var waterContact = DetectSwimCast();
            if (waterContact != _isSwimming)
            {
                Debug.Log("SWIM CHANGE!!!");
                _isSwimming = waterContact;
            }
        }
        
        private bool DetectSwimCast()
        {
            return Physics2D.BoxCast(_framePosition, Vector2.one * 0.5f, 0, Vector2.zero, 0f,
                Stats.WaterLayer);
        }
    }
}