using TalkingCrm.Model;

namespace TalkingCrm.DataProviderServices
{
    public interface IBirthdayTrackerModel
    {
        public void Init();
        public BirthdayTrackerModel GetNextBirthday(string userid);
        public BirthdayTrackerModel GetBirthday(string name);
        public bool AddBirthday(BirthdayTrackerModel b);
    }
}
