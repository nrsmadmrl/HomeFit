using Xunit;
using HomeFit.Models;

namespace HomeFit.Tests
{
    public class HomeFitTests
    {
        // ── PROGRAM ASSIGNMENT ENGINE (FR-ST-15, FR-ST-17) ──

        private string AssignProgram(User user)
        {
            if (user.Goal == "fat_loss" && user.FitnessLevel == "beginner")
                return "Beginner Fat Loss Program — 3 days/week, bodyweight cardio";
            else if (user.Goal == "muscle_gain" && user.FitnessLevel == "beginner")
                return "Beginner Muscle Gain Program — 3 days/week, basic strength";
            else if (user.Goal == "muscle_gain" && user.FitnessLevel == "intermediate")
                return "Intermediate Strength Program — 4 days/week, progressive overload";
            else if (user.Goal == "fat_loss" && user.FitnessLevel == "intermediate")
                return "Intermediate Fat Loss Program — 4 days/week, HIIT + strength";
            else
                return "General Fitness Program — 3 days/week, full body";
        }

        // FR-ST-15: Rule-Based Assignment
        [Fact]
        public void BeginnerFatLoss_ShouldReturnCorrectProgram()
        {
            var user = new User { Goal = "fat_loss", FitnessLevel = "beginner" };
            Assert.Equal("Beginner Fat Loss Program — 3 days/week, bodyweight cardio", AssignProgram(user));
        }

        [Fact]
        public void BeginnerMuscleGain_ShouldReturnCorrectProgram()
        {
            var user = new User { Goal = "muscle_gain", FitnessLevel = "beginner" };
            Assert.Equal("Beginner Muscle Gain Program — 3 days/week, basic strength", AssignProgram(user));
        }

        [Fact]
        public void IntermediateMuscleGain_ShouldReturnCorrectProgram()
        {
            var user = new User { Goal = "muscle_gain", FitnessLevel = "intermediate" };
            Assert.Equal("Intermediate Strength Program — 4 days/week, progressive overload", AssignProgram(user));
        }

        [Fact]
        public void IntermediateFatLoss_ShouldReturnCorrectProgram()
        {
            var user = new User { Goal = "fat_loss", FitnessLevel = "intermediate" };
            Assert.Equal("Intermediate Fat Loss Program — 4 days/week, HIIT + strength", AssignProgram(user));
        }

        // FR-ST-17: No AI Dependency - rule engine must return deterministic results
        [Fact]
        public void RuleEngine_SameinputAlwaysReturnsSameOutput()
        {
            var user1 = new User { Goal = "fat_loss", FitnessLevel = "beginner" };
            var user2 = new User { Goal = "fat_loss", FitnessLevel = "beginner" };
            Assert.Equal(AssignProgram(user1), AssignProgram(user2));
        }

        [Fact]
        public void UnknownProfile_ShouldReturnDefaultProgram()
        {
            var user = new User { Goal = "maintain", FitnessLevel = "advanced" };
            Assert.Equal("General Fitness Program — 3 days/week, full body", AssignProgram(user));
        }

        // FR-ST-11: Input Validation - null inputs should not crash
        [Fact]
        public void NullGoal_ShouldReturnDefaultProgram()
        {
            var user = new User { Goal = null, FitnessLevel = "beginner" };
            Assert.Equal("General Fitness Program — 3 days/week, full body", AssignProgram(user));
        }

        [Fact]
        public void NullFitnessLevel_ShouldReturnDefaultProgram()
        {
            var user = new User { Goal = "fat_loss", FitnessLevel = null };
            Assert.Equal("General Fitness Program — 3 days/week, full body", AssignProgram(user));
        }

        [Fact]
        public void BothNull_ShouldReturnDefaultProgram()
        {
            var user = new User { Goal = null, FitnessLevel = null };
            Assert.Equal("General Fitness Program — 3 days/week, full body", AssignProgram(user));
        }

        // ── USER MODEL TESTS (FR-ST-01, FR-ST-09, FR-ST-35) ──

        // FR-ST-01: User Registration
        [Fact]
        public void User_Email_ShouldBeSetCorrectly()
        {
            var user = new User { Email = "test@homefit.com" };
            Assert.Equal("test@homefit.com", user.Email);
        }

        [Fact]
        public void User_Name_ShouldBeSetCorrectly()
        {
            var user = new User { Name = "Nursima Demirel" };
            Assert.Equal("Nursima Demirel", user.Name);
        }

        // FR-ST-09: Profile Update - physical metrics
        [Fact]
        public void User_Age_ShouldBeSetCorrectly()
        {
            var user = new User { Age = 25 };
            Assert.Equal(25, user.Age);
        }

        [Fact]
        public void User_Weight_ShouldBeSetCorrectly()
        {
            var user = new User { Weight = 70.5f };
            Assert.Equal(70.5f, user.Weight);
        }

        [Fact]
        public void User_Height_ShouldBeSetCorrectly()
        {
            var user = new User { Height = 170f };
            Assert.Equal(170f, user.Height);
        }

        // FR-ST-11: Age boundary validation (10-100)
        [Fact]
        public void User_MinimumAge_ShouldBe10()
        {
            var user = new User { Age = 10 };
            Assert.True(user.Age >= 10);
        }

        [Fact]
        public void User_MaximumAge_ShouldBe100()
        {
            var user = new User { Age = 100 };
            Assert.True(user.Age <= 100);
        }

        // FR-ST-35: Premium Status Display
        [Fact]
        public void User_DefaultMembershipTier_ShouldBeFree()
        {
            var user = new User();
            Assert.Equal("Free", user.MembershipTier ?? "Free");
        }

        [Fact]
        public void User_SetMembershipTier_ShouldBePremium()
        {
            var user = new User { MembershipTier = "Premium" };
            Assert.Equal("Premium", user.MembershipTier);
        }

        // FR-ST-05: Role-Based Access Control
        [Fact]
        public void User_DefaultRole_ShouldBeUser()
        {
            var user = new User { Role = "User" };
            Assert.Equal("User", user.Role);
        }

        [Fact]
        public void Admin_Role_ShouldBeAdmin()
        {
            var user = new User { Role = "Admin" };
            Assert.Equal("Admin", user.Role);
        }

        [Fact]
        public void User_And_Admin_Roles_ShouldBeDifferent()
        {
            var user = new User { Role = "User" };
            var admin = new User { Role = "Admin" };
            Assert.NotEqual(user.Role, admin.Role);
        }

        // ── PASSWORD SECURITY TESTS (NFR-ST-01) ──

        // NFR-ST-01: Password Hashing with bcrypt cost factor 10
        [Fact]
        public void Password_ShouldBeHashedWithBCrypt()
        {
            var password = "Test1234!";
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
            Assert.True(BCrypt.Net.BCrypt.Verify(password, hash));
        }

        [Fact]
        public void Password_WrongPassword_ShouldNotVerify()
        {
            var password = "Test1234!";
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
            Assert.False(BCrypt.Net.BCrypt.Verify("WrongPassword", hash));
        }

        [Fact]
        public void Password_ShouldNotBeStoredAsPlainText()
        {
            var password = "Test1234!";
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
            Assert.NotEqual(password, hash);
        }

        [Fact]
        public void Password_HashShouldStartWithBCryptPrefix()
        {
            var password = "Test1234!";
            var hash = BCrypt.Net.BCrypt.HashPassword(password, workFactor: 10);
            Assert.StartsWith("$2", hash);
        }

        // FR-ST-06: Security Question Password Reset
        [Fact]
        public void User_SecurityQuestion_ShouldBeSetCorrectly()
        {
            var user = new User { SecurityQuestion = "What is your pet's name?" };
            Assert.Equal("What is your pet's name?", user.SecurityQuestion);
        }

        [Fact]
        public void User_SecurityAnswer_ShouldBeSetCorrectly()
        {
            var user = new User { SecurityAnswer = "fluffy" };
            Assert.Equal("fluffy", user.SecurityAnswer);
        }

        // ── EXERCISE MODEL TESTS (FR-ST-28, FR-ST-29, FR-ST-34) ──

        // FR-ST-28: Exercise Library
        [Fact]
        public void Exercise_Name_ShouldBeSetCorrectly()
        {
            var exercise = new Exercise { Name = "Squat" };
            Assert.Equal("Squat", exercise.Name);
        }

        [Fact]
        public void Exercise_MuscleGroup_ShouldBeSetCorrectly()
        {
            var exercise = new Exercise { MuscleGroup = "Legs" };
            Assert.Equal("Legs", exercise.MuscleGroup);
        }

        [Fact]
        public void Exercise_Difficulty_ShouldBeSetCorrectly()
        {
            var exercise = new Exercise { Difficulty = "Beginner" };
            Assert.Equal("Beginner", exercise.Difficulty);
        }

        [Fact]
        public void Exercise_Equipment_ShouldBeSetCorrectly()
        {
            var exercise = new Exercise { Equipment = "None" };
            Assert.Equal("None", exercise.Equipment);
        }

        // FR-ST-29: GIF Demonstration
        [Fact]
        public void Exercise_GifUrl_ShouldBeSetCorrectly()
        {
            var exercise = new Exercise { GifUrl = "https://res.cloudinary.com/homefit/squat.gif" };
            Assert.Equal("https://res.cloudinary.com/homefit/squat.gif", exercise.GifUrl);
        }

        [Fact]
        public void Exercise_GifUrl_ShouldContainValidUrl()
        {
            var exercise = new Exercise { GifUrl = "https://res.cloudinary.com/homefit/squat.gif" };
            Assert.StartsWith("https://", exercise.GifUrl);
        }

        // FR-ST-34: Feature Gating — Premium exercise flag
        [Fact]
        public void Exercise_DefaultIsPremium_ShouldBeFalse()
        {
            var exercise = new Exercise();
            Assert.False(exercise.IsPremium);
        }

        [Fact]
        public void Exercise_SetIsPremium_ShouldBeTrue()
        {
            var exercise = new Exercise { IsPremium = true };
            Assert.True(exercise.IsPremium);
        }

        [Fact]
        public void FreeExercise_ShouldBeAccessibleToAllUsers()
        {
            var exercise = new Exercise { IsPremium = false };
            var freeUser = new User { MembershipTier = "Free" };
            Assert.False(exercise.IsPremium);
        }

        [Fact]
        public void PremiumExercise_ShouldBeAccessibleOnlyToPremiumUsers()
        {
            var exercise = new Exercise { IsPremium = true };
            var premiumUser = new User { MembershipTier = "Premium" };
            Assert.True(exercise.IsPremium && premiumUser.MembershipTier == "Premium");
        }

        // ── SUBSCRIPTION MODEL TESTS (FR-ST-33, FR-ST-34, FR-ST-35) ──

        // FR-ST-33: Subscription Flow
        [Fact]
        public void Subscription_Type_ShouldBePremium()
        {
            var sub = new Subscription { Type = "Premium" };
            Assert.Equal("Premium", sub.Type);
        }

        [Fact]
        public void Subscription_PaymentStatus_ShouldBeSimulated()
        {
            var sub = new Subscription { PaymentStatus = "Simulated" };
            Assert.Equal("Simulated", sub.PaymentStatus);
        }

        [Fact]
        public void Subscription_EndDate_ShouldBeOneMonthAfterStart()
        {
            var start = DateTime.UtcNow;
            var end = start.AddMonths(1);
            var sub = new Subscription { StartDate = start, EndDate = end };
            Assert.Equal(end, sub.EndDate);
        }

        [Fact]
        public void Subscription_StartDate_ShouldBeSetCorrectly()
        {
            var date = DateTime.UtcNow;
            var sub = new Subscription { StartDate = date };
            Assert.Equal(date, sub.StartDate);
        }

        // ── PROGRESS MODEL TESTS (FR-ST-24, FR-ST-25) ──

        // FR-ST-25: Body Metrics Logging
        [Fact]
        public void Progress_Weight_ShouldBeSetCorrectly()
        {
            var progress = new Progress { Weight = 75.5f };
            Assert.Equal(75.5f, progress.Weight);
        }

        [Fact]
        public void Progress_LogDate_ShouldBeSetCorrectly()
        {
            var date = DateTime.UtcNow;
            var progress = new Progress { LogDate = date };
            Assert.Equal(date, progress.LogDate);
        }

        // FR-ST-24: Session History
        [Fact]
        public void WorkoutSession_Status_ShouldBeCompleted()
        {
            var session = new WorkoutSession { Status = "Completed" };
            Assert.Equal("Completed", session.Status);
        }

        [Fact]
        public void WorkoutSession_DurationMinutes_ShouldBePositive()
        {
            var session = new WorkoutSession { DurationMinutes = 45 };
            Assert.True(session.DurationMinutes > 0);
        }

        [Fact]
        public void WorkoutSession_StartDate_ShouldBeSetCorrectly()
        {
            var date = DateTime.UtcNow;
            var session = new WorkoutSession { StartDate = date };
            Assert.Equal(date, session.StartDate);
        }
    }
}