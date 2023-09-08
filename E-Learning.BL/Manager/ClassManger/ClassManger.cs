using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using E_Learning.BL.DTO;
using E_Learning.DAL;
using Microsoft.IdentityModel.Tokens;

namespace E_Learning.BL
{
    public class ClassManger : IClassManger
    {
    private readonly    IUnitOfWork _UnitOfWork;

        public ClassManger(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public ICollection<UserClassDTO> GetAll()
        {
          return _UnitOfWork.classrepository.GetAll().Select(x => new UserClassDTO { Id = x.Id, Name = x.Name }).ToList();
                
                
                }

        public ICollection<UserClassDTO> GetAllByYear(int yearid)
        {
            return _UnitOfWork.classrepository.GetAllByYear(yearid).Select(x => new UserClassDTO { Id = x.Id, Name = x.Name }).ToList();
        }

        public GetCLassdto GetAllClassesRequists( int Classid)
        {

            var clas = _UnitOfWork.classrepository.getbyid(Classid);

            var userrequset =  _UnitOfWork.classrepository.GetUserClassRequists(Classid).Select(x => new GetUserCLassRequistsdto { classid = (int)x.Classid, Userid = x.Userid,  UserName = $" {x.user.FirstName}  {x.user.SecondName}  {x.user.LastName}" }).ToList();
            return new GetCLassdto { ClassName = clas.Name, getUserCLassRequistsdtos = userrequset };
        
        
        }

        public ICollection<UserYearDTO> GetAllYears()
        {

            return _UnitOfWork.classrepository.GetAllYears().Select(x => new UserYearDTO { Id = x.Id, Name = x.Name  , Classes=null}).ToList();

        }


        public int ClassRequistStatue ( List< ClassRequistStatueDto> classRequistStatueDto)
        {
            int save = 0 ;
            if(classRequistStatueDto.IsNullOrEmpty())
            {
                return -1;
            }

            foreach (var request in classRequistStatueDto)
            {

                if (request.state == false)
                {

                    _UnitOfWork.classrepository.DeleteClassrequist(request.Classid, request.Userid);

                }
                
                if(request.state == true)
                {
                    var user = _UnitOfWork._Userrepository.GetUser(request.Userid);
                    var Class = _UnitOfWork.classrepository.getbyid(request.Classid);
                    if (user != null && Class !=null)
                    {
                        user.Classes.Add(Class);
                        _UnitOfWork.classrepository.DeleteClassrequist(request.Classid, request.Userid);


                        save = +  _UnitOfWork.SaveChanges();
                    }

                }
            }
            return save;

        }


        public int AddClassRequist( AddClassRequistDto addClassRequistDto)
        {


            _UnitOfWork.classrepository.AddClassrequist(addClassRequistDto.ClassId, addClassRequistDto.Userid);
            return _UnitOfWork.SaveChanges();
        }

        public int DeleteUserFromClass(AddClassRequistDto addClassRequistDto)
        {
            var user = _UnitOfWork._Userrepository.GetUser(addClassRequistDto.Userid);
            var Class = _UnitOfWork.classrepository.getbyid(addClassRequistDto.ClassId);
            if (user != null && Class != null)
            {
                user.Classes.Remove(Class);


            }
            return _UnitOfWork.SaveChanges();
        }







    }
}
