using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testproject
{
    internal class SongNode
    {
        public string FilePath { get; set; } // Đường dẫn bài hát
        public string FileName { get; set; } // Tên bài hát
        public SongNode Next { get; set; }   // Node tiếp theo
        public SongNode Prev { get; set; } // Node trước đó

        public SongNode(string filePath, string fileName)   //Constructor(Phương thức khởi tạo đối tượng SongNode
        {
            FilePath = filePath;   //Gán giá trị cho thuộc tính FilePath
            FileName = fileName;   //Gán giá trị cho thuộc tính FileName
            Next = null;   //Khởi tạo Node tiếp theo là null
            Prev = null;   //Khởi tạo Node trước đó là null
        }
        public override string ToString()   //Ghi đè phương thức ToString của lớp Object để trả về tên bài hát
        {
            return FileName;
        }
    }
}
