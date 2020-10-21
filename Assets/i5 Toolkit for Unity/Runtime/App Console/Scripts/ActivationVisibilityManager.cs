namespace i5.Toolkit.Core.AppConsole
{
    /// <summary>
    /// Visibility manager which changes the visibility by activating/deactivating the gameobject
    /// </summary>
    public class ActivationVisibilityManager : VisibilityManager
    {
        /// <summary>
        /// Gets or sets the current visibility of the gameobject
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                return gameObject.activeSelf;
            }
            set
            {
                gameObject.SetActive(value);
            }
        }
    }
}