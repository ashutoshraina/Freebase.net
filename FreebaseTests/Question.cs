using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreebaseTests
    {
    public class Question
        {
        public String type { get; set; }
        public String name { get; set; }
        public Dictionary<Object, Object> album { get; set; }
        public Question ()
            {
            album = new Dictionary<Object, Object>();
            }
        }
    }
