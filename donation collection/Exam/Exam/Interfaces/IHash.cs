namespace Exam.Interfaces
{
    public interface IHash
    {
        public string Hash(string pwd, string salt);

        public bool verify(string pwd, string hashedPwd, string salt);

    }
}
