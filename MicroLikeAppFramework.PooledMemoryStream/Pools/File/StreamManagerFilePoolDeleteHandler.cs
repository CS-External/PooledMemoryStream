using System.IO;
using System.Threading;

namespace MicroLikeAppFramework.PooledMemoryStreams.Pools.File
{
    public class StreamManagerFilePoolDeleteHandler
    {
        private FileStream m_Stream;
        private int m_RefCount = 0;
        private int m_FreeBlockCount;

        public StreamManagerFilePoolDeleteHandler(FileStream p_Stream, int p_FreeBlockCount)
        {
            m_Stream = p_Stream;
            m_FreeBlockCount = p_FreeBlockCount;
        }

        public int GetFreeBlockCount()
        {
            return m_FreeBlockCount;
        }

        public Stream GetStream()
        {
            return m_Stream;
        }

        public void IncRefCount()
        {
            Interlocked.Increment(ref m_RefCount);
            Interlocked.Decrement(ref m_FreeBlockCount);
        }

        public void DecRefCount()
        {
            Interlocked.Decrement(ref m_RefCount);

            if (m_RefCount > 0)
                return;

            if (m_Stream == null)
                return;

            string l_FileName = m_Stream.Name;
            m_Stream.Dispose();
            m_Stream = null;
            DirectoryUtils.SafeDelete(new FileInfo(l_FileName));
        }
    }
}
