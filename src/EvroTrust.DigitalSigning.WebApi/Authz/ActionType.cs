namespace EvroTrust.DigitalSigning.WebApi.Authz
{
    /// <summary>
    /// The enum containing actions(rights) that can be permitted to the user roles.
    /// </summary>
    internal enum ActionType : byte
    {
        CanRegisterCandidate = 1,
        CanAssignTask = 2,
        CanUploadSolution = 3,
        CanReviewSolution = 4,
        CanTakeFinalDecision = 5,
    }
}
