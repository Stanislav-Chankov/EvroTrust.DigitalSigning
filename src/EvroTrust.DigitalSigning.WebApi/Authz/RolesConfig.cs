namespace EvroTrust.DigitalSigning.WebApi.Authz
{
    internal static class RolesConfig
    {
        public static Dictionary<ActionType, Dictionary<RoleType, bool>> RoleActions => new Dictionary<ActionType, Dictionary<RoleType, bool>>
        {
            [ActionType.CanRegisterCandidate] = new Dictionary<RoleType, bool>
            {
                [RoleType.HumanResources] = true,
                [RoleType.Reviewer] = false,
                [RoleType.Candidate] = true,
                [RoleType.Director] = false,
            },
            [ActionType.CanAssignTask] = new Dictionary<RoleType, bool>
            {
                [RoleType.HumanResources] = false,
                [RoleType.Reviewer] = true,
                [RoleType.Candidate] = false,
                [RoleType.Director] = false,
            },
            [ActionType.CanUploadSolution] = new Dictionary<RoleType, bool>
            {
                [RoleType.HumanResources] = false,
                [RoleType.Reviewer] = false,
                [RoleType.Candidate] = true,
                [RoleType.Director] = false,
            },
            [ActionType.CanReviewSolution] = new Dictionary<RoleType, bool>
            {
                [RoleType.HumanResources] = false,
                [RoleType.Reviewer] = true,
                [RoleType.Candidate] = false,
                [RoleType.Director] = false,
            },
            [ActionType.CanTakeFinalDecision] = new Dictionary<RoleType, bool>
            {
                [RoleType.HumanResources] = false,
                [RoleType.Reviewer] = false,
                [RoleType.Candidate] = false,
                [RoleType.Director] = true,
            },
        };
    }
}
