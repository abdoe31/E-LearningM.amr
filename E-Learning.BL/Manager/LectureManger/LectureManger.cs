using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.BL;

public class LectureManger : ILectureManger
{
    private readonly IUnitOfWork _UnitOfWork;
    private readonly ELearningContext _eLearningContext;

    public LectureManger(IUnitOfWork unitOfWork, ELearningContext _eLearningContext)
    {
        _UnitOfWork = unitOfWork;
        this._eLearningContext = _eLearningContext;

    }

    public int AddAcessToUser(List<AddLectureAcessDto> addLectureAcessDtos , string name)
    {
        if (addLectureAcessDtos.IsNullOrEmpty())
        {
            return -1;
        }

        List<UserLecture> UserLectures = new List<UserLecture>();

        foreach (var item in addLectureAcessDtos)
        {
            var lec = _eLearningContext.Lectures.Where(x => x.Id == item.Lectureid).FirstOrDefault();
            if(lec == null)
            {

                return -2;
            }
            var oldaccess = _eLearningContext.UserLectures.Where(x => x.StudentId == item.UserId &&
            x.Lectureid == item.Lectureid).FirstOrDefault();
            if(oldaccess != null)
            {
                if (oldaccess.Start == null || oldaccess.End > DateTime.Now)
                {

                    return -2;
                }
                else
                {
                    UserLecture userLecture = new UserLecture
                    {
                        Lectureid = item.Lectureid,
                        AcessType = item.AcessType,
                        StudentId = item.UserId,
                        QuizRequired = false,
                        Duration = item.Duration ,
                        Createdby = name,
                        Createddate = DateTime.Now
                    };

                    UserLectures.Add(userLecture);
                }

            }else
            {
                UserLecture userLecture = new UserLecture
                {
                    Lectureid = item.Lectureid,
                    AcessType = item.AcessType,
                    StudentId = item.UserId,
                    QuizRequired = item.quizrequird,
                    Duration = item.Duration , Createdby=name , Createddate = DateTime.Now
                };

                UserLectures.Add(userLecture);
            }
            
        }


        _UnitOfWork._UserLecturerepository.AddALL(UserLectures);
        return _UnitOfWork.SaveChanges();
    }

    public int addLecture(AddLectureDTO addlecturedto)

    {


        Lecture lecture = new Lecture
        {
            Header = addlecturedto.Header.Trim(),
            Assighnmentid = addlecturedto.Assighnmentid,
            Quizid = addlecturedto.Quizid
        ,
            Classid = addlecturedto.Classid,
            number = addlecturedto.number, VideoParts = addlecturedto.addvideos.Select(x => new VideoPart { number = x.number, Url = x.link, PartHeader = x.PartHeader }).ToList()
            , Videofiles= addlecturedto.addFiles.Select(x => new Videofiles { number = x.number, Path = x.Path, PartHeader = x.PartHeader , UpdatedBy="Z " }).ToList()
        };
        _UnitOfWork.lecturerepository.Add(lecture);
        return _UnitOfWork.SaveChanges();
    }



    public int DeleteLecture(Deletedto deletedto)
    {

        var lecture = _eLearningContext.Lectures.Where(x => x.Id == deletedto.id).FirstOrDefault();
        if (lecture == null || lecture.Header != deletedto.name)
        {

            return -1;
        }


        _UnitOfWork.lecturerepository.Delete(lecture);
        return _UnitOfWork.SaveChanges();
    }

    public List<Codegenerateddto> GenerateCodes(PostCodegenerateddto postCodegenerateddto , string name )
    {
        int l = 6;

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();

        var codes = new List<LectureCode>();

        for (int i = 0; i < postCodegenerateddto.NumberofCode; i++)
       
        {

            var code = new string(Enumerable.Repeat(chars, l).Select(s => s[random.Next(s.Length)]).ToArray());

            var codeexist = _eLearningContext.LectureCodes.Any(x => x.Code == code);

            while(codeexist)
            {
                code = new string(Enumerable.Repeat(chars, l).Select(s => s[random.Next(s.Length)]).ToArray());
                codeexist = _eLearningContext.LectureCodes.Any(x => x.Code == code);
            }

            var one = new LectureCode();



            if (postCodegenerateddto.CodeTybe == CodeTybe.Super)
            {
                one.CodeTybe = CodeTybe.Super;



            }else if (postCodegenerateddto.CodeTybe == CodeTybe.Master)
            {
                one.CodeTybe = CodeTybe.Master;
                one.Classid = postCodegenerateddto.classid;



            }
            else if (postCodegenerateddto.CodeTybe == CodeTybe.lecture)
            {
                one.CodeTybe = CodeTybe.lecture;
                one.Lectureid = postCodegenerateddto.Lectureid;



            }




            one.Code = code;
            one.GeneratedAt = DateTime.Now;
            one.GeneratedBy = name;
            one.duration = postCodegenerateddto.duration;
            
            one.QuizRequired = postCodegenerateddto.QuizRequird;
            
            codes.Add(one);
        }
        Lecture  lect = null; 
        Class cl = null;
        if (postCodegenerateddto.CodeTybe == CodeTybe.lecture)
        {
            lect = _eLearningContext.Lectures.Where(x => x.Id == postCodegenerateddto.Lectureid).Include(x=>x.Class).FirstOrDefault();
            cl = lect.Class;

        }
        else    if (postCodegenerateddto.CodeTybe == CodeTybe.Master)
        {
            cl = _eLearningContext.Classes.Where(x => x.Id == postCodegenerateddto.classid).FirstOrDefault();

        }


        _eLearningContext.LectureCodes.AddRange(codes);
        _UnitOfWork.SaveChanges();

        return codes.Select(x => new Codegenerateddto { LectureName = lect != null ? lect.Header : null , Code = x.Code  , ClassName= cl !=null ? cl.Name : null , Codetype= x.CodeTybe.ToString() }).ToList();

    }

    public List<GetCodesDTO> GetCodes(int Lectureid)


    {

        var codes = _eLearningContext.LectureCodes.Where(x => x.Lectureid == Lectureid).Include(x=>x.Student);

        if (codes.IsNullOrEmpty())
        {
            return null;
        }
        return codes.Select(x => new GetCodesDTO { Code = x.Code, CodeId = x.Id, Used = x.Used, Usedate = x.Usedate,

             UserName = x.Student != null ?  $"{x.Student.FirstName}  {x.Student.SecondName}  {x.Student.LastName}" :null
        }).ToList();



    }



    public List<GetCodesDTO> GetCodesv2()


    {

        var codes = _eLearningContext.LectureCodes.Include(x => x.Student);

        if (codes.IsNullOrEmpty())
        {
            return null;
        }
        return codes.Select(x => new GetCodesDTO
        {
            Code = x.Code,
            CodeId = x.Id,
            Used = x.Used,
            Usedate = x.Usedate,
              Lecturename = x.Lecture != null ? $"{x.Lecture.Header}" : null
            ,UserName = x.Student != null ? $"{x.Student.FirstName}  {x.Student.SecondName}  {x.Student.LastName}" : null 

            , Createdby = x.GeneratedBy !=null ? x.GeneratedBy.ToString() : null
            , Createddate = x.GeneratedAt !=null ? x.GeneratedAt : null  
        }).OrderByDescending(x=>  x.Used) .ToList();



    }



    public LectureAttendanceDTO GetLectureAttendance(int lectureId)
    {

        var lec = _eLearningContext.Lectures.Where(x => x.Id == lectureId)  .Include(x => x.UserLectures). ThenInclude(x => x.Student).Include(x=>x.UserLectures).ThenInclude(x=>x.Place).FirstOrDefault();
      if (lec == null)
        {
            return null;
        }
        
        var lecture = _eLearningContext.UserLectures.Where(x => x.Lectureid == lectureId).Include(x=>x.Lecture).Include(x=>x.Student);


        return new LectureAttendanceDTO
        {

            LectureName = lec.Header,
            userLectureAttendances = lec.UserLectures.Select(x =>
        new UserLectureAttendance
        {
            UserName = $"{x.Student.FirstName}  {x.Student.SecondName}  {x.Student.LastName}",
            start = x.Start,
            end = x.End
            , id = x.Id, 
            accesstype = x.AcessType.ToString(), accessby = x.Createdby != null ? x.Createdby.ToString() : null
            ,
            accessdate = x.Createddate !=null ?  x.Createddate : null
            , LectureType = x.LectureType.ToString(), Place = x.Place?.name , assigmentattent = x.AssighmentSolved , Assigmentgrade = x.AssighmentGrade
        }

        ).ToList()
        };

    }


    public List<LectureDetailsDto> GetLectureList(int Classid)
    {
        var Lectures = _eLearningContext.Lectures.Where(x => x.Classid == Classid).OrderBy(x=>x.number);


        if (Lectures.IsNullOrEmpty())
        {


            return null;
        }


        return Lectures.Select(x => new LectureDetailsDto { Header = x.Header, Assighnmentid = x.Assighnmentid, AssighnmentName = x.Assighnment.Header,
            ClassName = x.Class.Name, LectureId = x.Id, Quizid = x.Quizid, QuizName = x.Quiz.Header }).ToList();
    }

    public UsersCLass GetLectureWithUsers(int Lectureid)
    {

        var lecture = _eLearningContext.Lectures.Where(x => x.Id == Lectureid).FirstOrDefault();
        if (lecture is null)
        {
            return null;
        }
        var users = _UnitOfWork._Userrepository.GetStudentsByClassToLecture((int)lecture.Classid).Users.Where(x =>

        {  if (x.UserLectures.Where(y => y.Lectureid == Lectureid).FirstOrDefault() == null)
            {
                return true;

            }
                if  (x.UserLectures.Where(x => x.Lectureid == Lectureid).FirstOrDefault()?.Start != null && DateTime.Now > x.UserLectures.Where(x => x.Lectureid == Lectureid).FirstOrDefault()?.End)
            {

                return true;
            }
            return false;
            
            
            }
        
        
        
        
        ).ToList();



        if (users.IsNullOrEmpty())
        {


            return null;
        }
        UsersCLass U = new UsersCLass { LectureId = Lectureid, LectureName = lecture.Header, users = users.Select(x => new Users { id = x.Id,
            userName = $"{x.FirstName}  {x.SecondName}  {x.LastName}", ParentPhone = x.ParentPhoneNumber, Phone = x.StudentPhoneNumber

        }).ToList() };

        return U;

    }

    public UserLecturedto GetStudentLectureAttendence(string Studentid)
    {
        var user = _UnitOfWork._Userrepository.GetUser(Studentid);

        if (user == null)
        {
            return null;
        }

        var userattend = new UserLecturedto { StudentName = $"{user.FirstName}  {user.SecondName}  {user.LastName}",

            lectureuserAcessds = user.UserLectures.Select(x => new lectureuserAcessd { Lectureid = x.Id, AcessType = x.AcessType.ToString(), Start = x.Start

            , End = x.End
             , LectureName = x.Lecture?.Header , LectureType= x.LectureType?.ToString() , Place = x.Place?.name }).ToList()
        };


        return userattend;
    }

    public int UpdateLecture(UpdateLectureDto updateLectureDto)
    {
        var lecture = _eLearningContext.Lectures.Where(x => x.Id == updateLectureDto.LectureId).FirstOrDefault();

        if (lecture == null) { return -1; }

        lecture.Classid = updateLectureDto.Classid;
        lecture.Header = updateLectureDto.Header;
        lecture.Quizid = updateLectureDto.Quizid;
        lecture.Assighnmentid = updateLectureDto.Assighnmentid;
        return _UnitOfWork.SaveChanges();

    }


    public int UpdateLecture2(UpdateLectureDto updateLectureDto)
    {
        var lecture = _eLearningContext.Lectures.Where(x => x.Id == updateLectureDto.LectureId).FirstOrDefault();

        if (lecture == null) { return -1; }

        lecture.Classid = updateLectureDto.Classid;
        lecture.Header = updateLectureDto.Header;
        lecture.Quizid = updateLectureDto.Quizid;
        lecture.Assighnmentid = updateLectureDto.Assighnmentid;
        return _UnitOfWork.SaveChanges();

    }


    public int AcessLectureByCode(string code, string userid)
    {


        var lecturecode = _eLearningContext.LectureCodes.Where(x => x.Code == code  && x.Used==false).Include(x => x.Lecture).FirstOrDefault();
        if (lecturecode== null)
        {

            return -1;
        }
        lecturecode.StudentId = userid;

        lecturecode.Used = true;
        lecturecode.Usedate = DateTime.Now;

        _UnitOfWork._Userrepository.GetUser(userid).UserLectures.Add( new UserLecture {  Lectureid=lecturecode.Lectureid , LectureType = LectureType.Online ,  AcessType= AcessType.Code,
            Duration= lecturecode.duration, StudentId=  userid , QuizRequired= (bool)lecturecode.QuizRequired 
        });
        return _UnitOfWork.SaveChanges() ; 
    }




    public int AcessLectureByCodev2(LectureCode lecturecode, string userid , int lectureidd)
    {


        lecturecode.StudentId = userid;

        lecturecode.Used = true;
        lecturecode.Usedate = DateTime.Now;
        lecturecode.Lectureid = lectureidd;
        _UnitOfWork._Userrepository.GetUser(userid).UserLectures.Add(new UserLecture
        {
            Lectureid = lectureidd,
            AcessType = AcessType.Code,
            Duration = lecturecode.duration,
            StudentId = userid,
            QuizRequired = (bool)lecturecode.QuizRequired ,
            LectureType = LectureType.Online
        });
        return _UnitOfWork.SaveChanges();
    }

    public StartendLecture startWatching(int userLectureid)
    {
        var userlecture = _eLearningContext.UserLectures.Where(x=>x.Id==userLectureid).FirstOrDefault();

        userlecture.Start = DateTime.Now;

        if(userlecture.Duration != null)
        {
            userlecture.End = DateTime.Now.AddDays((int)userlecture.Duration);


        }

       var state =  _UnitOfWork.SaveChanges();
        if (state > 0)
        {


            return new StartendLecture { start = userlecture.Start, end = userlecture.End };
        }
        return null;


    }

    public List<Selectdto> getLecturetowatch(string userid, int classid )
    {


      var lectures =    _eLearningContext.UserLectures.Where(x=>(x.Start==null || x.End> DateTime.Now)  && x.StudentId==userid && x.Lecture.Classid==classid && x.LectureType == LectureType.Online ).Include(x=>x.Lecture).Include(x=> x.Lecture.VideoParts).Include(x=>x.Lecture.Quiz).Include(x=>x.Lecture.Assighnment).OrderBy(x=>x.Lecture.number);

        return lectures.Select(x =>  new Selectdto { id = x.Id, name = x.Lecture.Header }).ToList();




    }
}