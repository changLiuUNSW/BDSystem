using System.Web;

namespace DateAccess.Services.Excel
{
    public abstract class ExcelCopier
    {
        protected delegate bool Copy<T>(ref T entity, int i, int j, string text, string sheet);
        protected delegate void Save<in T>(T entity);

        public abstract void Begin();

        protected ExcelCopier(string file)
        {
            File = file;
        }

        protected string File { get; set; }

        protected object[,] Read(string sheet, string start, string end)
        {
            return ExcelReader.Read(File, sheet, start, end);
        }

        /// <summary>
        /// read and construct mutiple object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="copy"></param>
        /// <param name="save"></param>
        protected void Read<T>(string sheet, string start, string end, Copy<T> copy, Save<T> save) where T : new()        
        {
            var list = Read(sheet, start, end);

            for (var i = 1; i <= list.GetLength(0); i++)
            {
                var entity = new T();
                var doSave = true;

                for (var j = 1; j <= list.GetLength(1); j++)
                {
                    if (list[i, j] == null)
                        continue;

                    var text = list[i, j].ToString();

                    doSave = copy(ref entity, i, j, text, sheet);

                    if (!doSave)
                        break;
                }

                if (!doSave)
                    continue;

                save(entity);
            }
        }

        /// <summary>
        /// read and construct single object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sheet"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="copy"></param>
        /// <returns></returns>
        protected T ReadSingle<T>(string sheet, string start, string end, Copy<T> copy) where T : new()
        {
            var list = Read(sheet, start, end);

            var entity = new T();

            for (var i = 1; i <= list.GetLength(0); i++)
            {
                for (var j = 1; j <= list.GetLength(1); j++)
                {
                    if (list[i, j] == null)
                        continue;

                    var text = list[i, j].ToString();

                    copy(ref entity, i, j, text, sheet);
                }
            }

            return entity;
        }

        protected void ReadVertical<T>(string sheet, string start, string end, Copy<T> copy, Save<T> save)
            where T : new()
        {
            var list = Read(sheet, start, end);

            var entity = new T();
            for (var i = 1; i <= list.GetLength(0); i++)
            {
                var text = list[i, 1].ToString();
                copy(ref entity, i, 1, text, sheet);
            }

            save(entity);
        }

        protected void ReadHorizontally<T>(string sheet, string start, string end, Copy<T> copy, Save<T> save) where T: new()
        {
            var list = Read(sheet, start, end);

            for (var i = 1; i <= list.GetLength(1); i++)
            {
                var entity = new T();
                var doSave = true;

                for (var j = 1; j <= list.GetLength(0); j++)
                {
                    if (list[j, i] == null)
                        continue;

                    var text = list[j, i].ToString();
                    doSave = copy(ref entity, i, j, text, sheet);

                    if (!doSave)
                        break;
                }

                if (!doSave)
                    continue;

                save(entity);
            }
        }
    }
}
