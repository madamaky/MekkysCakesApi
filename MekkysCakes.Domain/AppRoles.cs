namespace MekkysCakes.Domain
{
    public static class AppRoles
    {
        public const string TopTierHuman = nameof(TopTierHuman);
        public const string SuperAdmin = nameof(SuperAdmin);
        public const string Admin = nameof(Admin);

        public const string AllAdmins = $"{TopTierHuman},{SuperAdmin},{Admin}";
    }
}
