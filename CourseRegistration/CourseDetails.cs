using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseRegistration
{
    class CourseDetails : Base4Model
    {
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }

        public string CourseInfo { get; set; }

        public CourseDetails(string courseCode, string courseTitle)
        {
            CourseCode = courseCode;
            CourseTitle = courseTitle;
            CourseInfo = courseCode + " " + courseTitle;
        }
    }
}
