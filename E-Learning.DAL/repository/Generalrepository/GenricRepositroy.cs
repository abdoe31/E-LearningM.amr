using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.DAL
{
    public class GenricRepositroy<T> : IGenricRepositroy<T> where T : class
    {
        private readonly ELearningContext _eLearningContext;
        public GenricRepositroy(ELearningContext eLearningContext)
        {

            _eLearningContext = eLearningContext;
        }
        public int Add(T item)
        {
            if (item == null) { return 0; }

          _eLearningContext.Set<T>().Add(item);

            return 1;

        }


        public int AddALL(ICollection<T> items)
        {
            if (items.IsNullOrEmpty()) { return 0; }

      _eLearningContext.Set<T>().AddRange(items);

            return 1;
        }

        public int Delete(T item)
        {
            if (item == null) { return 0; }

    _eLearningContext.Set<T>().Remove(item);

           return 1;
        }

        public int DeleteALL(ICollection<T> items)
        {
            if (items.IsNullOrEmpty()) { return 0; }

           _eLearningContext.Set<T>().RemoveRange(items);

            return 1;
        }


        public int Update(T item)
        {
            if (item == null) { return 0; }

            _eLearningContext.Set<T>().Update(item);

            return 1;
        }




    public    int UpdateAll(ICollection<T> items)
        {

            if (items.IsNullOrEmpty() ) { return 0; }

            _eLearningContext.Set<T>().UpdateRange(items);

            return 1;


        }

    }

}