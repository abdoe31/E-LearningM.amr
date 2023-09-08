namespace E_Learning.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserrepository _Userrepository { get; }
        public IClassrepository classrepository { get; }
        private readonly ELearningContext _eLearningContext;

        public UnitOfWork(IUserrepository userrepository, IClassrepository classrepository, ELearningContext eLearningContext, IUserAnswerrepository userAnswerrepository, ILecturerepository lecturerepository, IUserLecturerepository userLecturerepository)
        {
            _Userrepository = userrepository;
            this.classrepository = classrepository;
            _eLearningContext = eLearningContext;
            _userAnswerrepository = userAnswerrepository;
            this.lecturerepository = lecturerepository;
            _UserLecturerepository = userLecturerepository;
        }

        public IUserAnswerrepository _userAnswerrepository { get; }

      public   ILecturerepository lecturerepository { get; }
      public   IUserLecturerepository _UserLecturerepository { get; }



        public   int SaveChanges()
        {



         return   _eLearningContext.SaveChanges();
        }
    }
}