using Unity1week202112.Data;

namespace Unity1week202112.Domain.User
{
    public class UserRepository
    {
        private UserData _data;

        public UserRepository()
        {
            _data = new UserData();
        }

        public void Save(UserData userData)
        {
            _data = userData;
        }

        public UserData Load()
        {
            return _data;
        }

        private void Delete()
        {
            _data = new UserData();
        }
    }
}
