namespace TarodevController
{
    public partial class PlayerController
    {
        private bool _isNoclip;
        private bool CanNoclip => Stats.AllowNoclip;
        
        private void CalculateNoclip()
        {
            if (!CanNoclip) return;
            if (_frameInput.NoClipDown)
            {
                ToggleNoclip();
            }
        }
        
        private void MoveNoclip()
        {
            SetVelocity(_frameInput.Move * Stats.NoclipSpeed);
        }

        private void ToggleNoclip()
        {
            _isNoclip = !_isNoclip;
            _rb.isKinematic = _isNoclip;
        }
    }
}