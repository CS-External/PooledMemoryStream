using System;
using System.IO;

namespace MicroLikeAppFramework.PooledMemoryStreams.Pools.File
{
    public class StreamManagerFilePool: StreamManagerPool
    {
        private const int CONST_BLOCKSIZE = 1 * 1024 * 1024; // 1 MB

        private readonly String m_CacheDirectory;
        private readonly int m_BlocksPerFile;
        private readonly int m_BlockSize;


        private StreamManagerFilePoolDeleteHandler m_CurrentDeleteHandler;

        private readonly object m_GetLock;

        public StreamManagerFilePool(String p_Name, string p_CacheDirectory, int p_BlocksPerFile) : this(p_Name, Int32.MaxValue, p_CacheDirectory, p_BlocksPerFile)
        {
            
        }

        public StreamManagerFilePool(String p_Name, int p_MaxBlockCount, string p_CacheDirectory, int p_BlocksPerFile) : base(p_Name, p_MaxBlockCount)
        {
            m_CacheDirectory = p_CacheDirectory;
            m_BlocksPerFile = p_BlocksPerFile;

            if (m_BlocksPerFile == 1)
                m_BlockSize = Int32.MaxValue;
            else
                m_BlockSize = CONST_BLOCKSIZE;

            m_GetLock = new object();
            DirectoryUtils.EnsureCreated(m_CacheDirectory);
            DirectoryUtils.Cleanup(m_CacheDirectory);
            
        }

        public override int GetBlockSize()
        {
            return m_BlockSize;
        }

        
        protected override MemoryBlock DoGetBlock()
        {
            lock (m_GetLock)
            {
                
                if (m_CurrentDeleteHandler == null)
                {
                    // Create new TempFile
                    string l_FilePath = Path.Combine(m_CacheDirectory, Guid.NewGuid().ToString());
                    FileStream l_FileStream = new FileStream(l_FilePath, FileMode.CreateNew);

                    if (m_BlocksPerFile != 1)
                    {
                        l_FileStream.SetLength(m_BlocksPerFile * GetBlockSize());
                    }
                    
                    m_CurrentDeleteHandler = new StreamManagerFilePoolDeleteHandler(l_FileStream, m_BlocksPerFile);

                }

                StreamManagerFilePoolDeleteHandler l_DeleteHandler = m_CurrentDeleteHandler;
                long l_Offset = (m_BlocksPerFile - l_DeleteHandler.GetFreeBlockCount()) * GetBlockSize();
                l_DeleteHandler.IncRefCount();

                if (l_DeleteHandler.GetFreeBlockCount() == 0)
                {
                    m_CurrentDeleteHandler = null;
                }

                return new FileMemoryBlock(this, l_DeleteHandler.GetStream(), l_Offset, l_DeleteHandler);
            }

            
           
        }

        protected override void DoReturnBlock(MemoryBlock p_Block)
        {

            // Delete the TempFile
            FileMemoryBlock l_Block = (FileMemoryBlock)p_Block;
            StreamManagerFilePoolDeleteHandler l_DeleteHandler = l_Block.DeleteHandler;
            l_DeleteHandler.DecRefCount();
        }
    }
}
