using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testproject
{
    internal class DoubleLinkedList       
    {
        public SongNode Head { get; private set; } // Node đầu tiên của danh sách
        public SongNode Tail { get; private set; } // Node cuối cùng của danh sách
        public SongNode Current { get; set; } // Node hiện tại của danh sách
        public int Count { get; private set; } // Số lượng Node trong danh sách
        public void AddSong(string filePath, string fileName)   //Phương thức thêm bài hát vào danh sách
        {
            SongNode newNode = new SongNode(filePath, fileName);   //Tạo một Node mới
            if (Head == null)   //Nếu danh sách rỗng
            {
                Head = newNode;   //Node mới là Node đầu tiên
                Tail = newNode;   //Node mới là Node cuối cùng
            }
            else   //Nếu danh sách không rỗng
            {
                Tail.Next = newNode;   //Node cuối cùng hiện tại trỏ đến Node mới
                newNode.Prev = Tail;   //Node mới trỏ về Node cuối cùng hiện tại
                Tail = newNode;   //Node mới là Node cuối cùng
            }
            Count++;   //Tăng số lượng Node trong danh sách
        }
        public void RemoveSong(int index)   // Phương thức xóa bài hát khỏi danh sách
        {
            if (Head == null || index < 0) return;
            SongNode current = Head;
            int count = 0;

            while (current != null)
            {
                if (count == index)
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    if (current == Head)
                        Head = current.Next;
                    if (current == Tail)
                        Tail = current.Prev;

                    return;
                }
                current = current.Next;
                count++;
            }
        }   
        public SongNode GetNodeAt(int index)   // Lấy bài hát được chỉ định 
        {
            if (index < 0 || index >= Count)   // Kiểm tra nếu index không hợp lệ
                return null;   // Trả về null nếu không hợp lệ

            SongNode current = Head;   // Bắt đầu từ Node đầu tiên
            for (int i = 0; i < index; i++)   // Duyệt qua danh sách cho đến vị trí mong muốn
            {
                current = current.Next;   // Di chuyển đến Node tiếp theo
            }
            return current;   // Trả về Node tại vị trí index
        }
        public void SwapSong(int index1, int index2)   // Phương thức đổi vị trí 2 bài hát trong danh sách
        {
            if (index1 == index2 || Head == null) return;   // Nếu 2 bài hát giống nhau hoặc danh sách rỗng thì thoát khỏi phương thức

            SongNode node1 = null, node2 = null, current = Head;   // Khởi tạo Node hiện tại là Node đầu tiên
            int count = 0;   // Biến đếm  

            while (current != null)   // Tìm hai Node cần hoán đổi
            {
                if (count == index1) node1 = current;   // Nếu tìm thấy Node đầu tiên
                if (count == index2) node2 = current;   // Nếu tìm thấy Node thứ hai
                current = current.Next;   // Chuyển sang Node tiếp theo
                count++;   // Tăng biến đếm
            }

            if (node1 == null || node2 == null) return;   // Nếu một trong hai node không tồn tại, thoát

            if (node1.Next == node2)   //Nếu Node1 trước Node2
            {
                if (node1.Prev != null) node1.Prev.Next = node2;   // Node trước Node1 trỏ đến Node2
                if (node2.Next != null) node2.Next.Prev = node1;   // Node tiếp theo Node2 trỏ về Node1

                node2.Prev = node1.Prev;   // Node2 trước đó trỏ về Node1 trước đó
                node1.Next = node2.Next;   // Node1 tiếp theo trỏ về Node2 tiếp theo
                node1.Prev = node2;   // Node1 trước đó trỏ về Node2
                node2.Next = node1;   // Node2 tiếp theo trỏ về Node1
            }
            else if (node2.Next == node1)
            { 
                if (node2.Prev != null) node2.Prev.Next = node1;   // Node trước Node2 trỏ đến Node1
                if (node1.Next != null) node1.Next.Prev = node2;   // Node tiếp theo Node1 trỏ về Node2

                node1.Prev = node2.Prev;   // Node1 trước đó trỏ về Node2 trước đó
                node2.Next = node1.Next;   // Node2 tiếp theo trỏ về Node1 tiếp theo
                node2.Prev = node1;   // Node2 trước đó trỏ về Node1
                node1.Next = node2;   // Node1 tiếp theo trỏ về Node2
            }
            else
            {
                if (node1.Prev != null) node1.Prev.Next = node2;   // Node trước Node1 trỏ đến Node2
                if (node1.Next != null) node1.Next.Prev = node2;   // Node tiếp theo Node1 trỏ về Node2
                if (node2.Prev != null) node2.Prev.Next = node1;   // Node trước Node2 trỏ đến Node1    
                if (node2.Next != null) node2.Next.Prev = node1;   // Node tiếp theo Node2 trỏ về Node1

                SongNode tempPrev = node1.Prev;   // Node1 trước đó
                SongNode tempNext = node1.Next;   // Node1 tiếp theo
                node1.Prev = node2.Prev;   // Node1 trước đó trỏ về Node2 trước đó
                node1.Next = node2.Next;   // Node1 tiếp theo trỏ về Node2 tiếp theo
                node2.Prev = tempPrev;   // Node2 trước đó trỏ về Node1 trước đó
                node2.Next = tempNext;   // Node2 tiếp theo trỏ về Node1 tiếp theo
            }

            if (Head == node1) Head = node2;   // Nếu Node1 là Node đầu tiên thì Node2 là Node đầu tiên
            else if (Head == node2) Head = node1;   // Nếu Node2 là Node đầu tiên thì Node1 là Node đầu tiên

            if (Tail == node1) Tail = node2;   // Nếu Node1 là Node cuối cùng thì Node2 là Node cuối cùng
            else if (Tail == node2) Tail = node1;   // Nếu Node2 là Node cuối cùng thì Node1 là Node cuối cùng
        }
        public SongNode FindSong(string fileName)   //Phương thức tìm bài hát trong danh sách
        {
            if (Head == null) return null;   //Nếu danh sách rỗng thì thoát khỏi phương thức
            SongNode current = Head;   //Node hiện tại là Node đầu tiên
            while (current != null)   //Duyệt qua các Node trong danh sách
            {
                if (current.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))   //Nếu tìm thấy bài hát
                    return current;   //Thoát khỏi phương thức
                current = current.Next;   //Chuyển sang Node tiếp theo
            }
            return null;   //Nếu không tìm thấy bài hát thì trả về null
        }
        public void MoveNext()   //Phương thức chuyển sang Node tiếp theo
        {
            if (Current != null && Current.Next != null)   //Nếu Node hiện tại không phải Node cuối cùng
                Current = Current.Next;   //Chuyển sang Node tiếp theo
            else Current = null;   //Nếu Node hiện tại là Node cuối cùng thì gán Node hiện tại là null

        }
        public void MovePrev()   //Phương thức chuyển sang Node trước đó
        {
            if (Current != null && Current.Prev != null)   //Nếu Node hiện tại không phải Node đầu tiên
                Current = Current.Prev;   //Chuyển sang Node trước đó
            else Current = null;   //Nếu Node hiện tại là Node đầu tiên thì gán Node hiện tại là null
        }
        public void SortSongs()
        {
            if (Head == null || Head.Next == null) return;  // Nếu danh sách rỗng hoặc chỉ có một bài hát thì không cần sắp xếp

            SongNode current, index;
            string temp;

            for (current = Head; current != null; current = current.Next)
            {
                for (index = current.Next; index != null; index = index.Next)
                {
                    if (string.Compare(current.FileName, index.FileName) > 0)  // So sánh tên bài hát
                    {
                        // Hoán đổi tên bài hát
                        temp = current.FileName;
                        current.FileName = index.FileName;
                        index.FileName = temp;

                        // Hoán đổi đường dẫn bài hát
                        temp = current.FilePath;
                        current.FilePath = index.FilePath;
                        index.FilePath = temp;
                    }
                }
            }
        }

    }
}
