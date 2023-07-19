using TalkingCrm.DataProviderServices;
using TalkingCrm.Model;

namespace TalkingCrm.DataProvider
{
    public class InMemory: IBirthdayTrackerModel
    {
        static List<BirthdayTrackerModel> birthdays;
        public InMemory()
        {
            Init();
        }



        public BirthdayTrackerModel GetBirthday(string name)
        {
            BirthdayTrackerModel named = birthdays.FirstOrDefault(b => b.Name.Equals(name,StringComparison.CurrentCultureIgnoreCase));
            if (named != null)
            {
                if (named.DateofBirthday < DateTime.Today)
                {
                    named.DateofBirthday = new DateTime(DateTime.Today.Year + 1,
                        named.DateofBirthday.Month, named.DateofBirthday.Day);
                }
            }
            return named;
        }
        public BirthdayTrackerModel GetNextBirthday(string userid)
        {
            List<BirthdayTrackerModel> birthdaysSorted = birthdays.OrderBy(b => b.DayOfYear).ToList();
            int currentDayOfYear = DateTime.Today.DayOfYear;
            BirthdayTrackerModel next = birthdaysSorted.Find(b => b.DayOfYear >= currentDayOfYear);
            if (next == null && birthdaysSorted.Count > 0)
            {
                next = birthdaysSorted[0];
            }
            if (next != null)
            {
                if (next.DateofBirthday < DateTime.Today)
                {
                    next.DateofBirthday = new DateTime(DateTime.Today.Year + 1,
                        next.DateofBirthday.Month, next.DateofBirthday.Day);
                }
            }
            return next;
        }

        public bool AddBirthday(BirthdayTrackerModel b)
        {
            try
            {
                birthdays.Add(b);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public void Init()
        {
            if (birthdays == null)
            {
                birthdays = new List<BirthdayTrackerModel>();
                birthdays.Add(new BirthdayTrackerModel()
                {
                    Name = "John",
                    DateofBirthday = new DateTime(2020, 1, 1),
                    DayOfYear = (new DateTime(2020, 1, 1)).DayOfYear
                });
                birthdays.Add(new BirthdayTrackerModel()
                {
                    Name = "Mary",
                    DateofBirthday = new DateTime(2022, 2, 2),
                    DayOfYear = (new DateTime(2022, 2, 2)).DayOfYear
                });
                birthdays.Add(new BirthdayTrackerModel()
                {
                    Name = "Mark",
                    DateofBirthday = new DateTime(2021, 10, 10),
                        DayOfYear = (new DateTime(2021, 10, 10)).DayOfYear
                    });

                }
        }
    }

}
