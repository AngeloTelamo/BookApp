namespace ASI.Basecode.Resources.Constants
{
    /// <summary>
    /// Class for enumerated values
    /// </summary>
    public class Enums
    {
        /// <summary>
        /// API Result Status
        /// </summary>
        public enum Status
        {
            Success,
            Error,
            CustomErr,
        }

        /// <summary>
        /// Login Result
        /// </summary>
        public enum LoginResult
        {
            Success = 0,
            Failed = 1,
        }

        public enum Rating
        {
            OneStar = 1,
            TwoStars = 2,
            ThreeStar = 3,
            FourStars = 4,
            FiveStars = 5
        }
    }
}
