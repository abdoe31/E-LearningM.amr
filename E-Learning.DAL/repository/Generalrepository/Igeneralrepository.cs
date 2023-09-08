using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public interface IGenricRepositroy<T> where T : class
    {

        int Add(T item);
        int AddALL( ICollection <T> items);
        int DeleteALL(ICollection<T> items);
        int UpdateAll(ICollection<T> items);
        int Delete(T item);
        int Update(T item);


    }
}
