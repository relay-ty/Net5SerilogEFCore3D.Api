using System;

namespace Net5SerilogEFCore3D.Model.DomainCoreModels
{
    /// <summary>
    /// 定义领域实体基类
    /// </summary>
    public abstract class IntEntity
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; protected set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; protected set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDeleted { get; set; } = false;
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTimeOffset? DeletdTime { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool? IsEnable { get; protected set; } = true;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset? CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>        
        public Guid? CreateUserId { get; protected set; }

        /// <summary> 
        /// 修改时间
        /// </summary>
        public DateTimeOffset? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public Guid? ModifyUserId { get; protected set; }

        /// <summary>
        /// 重写方法 相等运算
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var compareTo = obj as IntEntity;
            if (ReferenceEquals(this, compareTo)) return true;
            if (compareTo is null) return false;
            return Id.Equals(compareTo.Id);
        }
        /// <summary>
        /// 重写方法 实体比较 ==
        /// </summary>
        /// <param name="a">领域实体a</param>
        /// <param name="b">领域实体b</param>
        /// <returns></returns>
        public static bool operator ==(IntEntity a, IntEntity b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }
        /// <summary>
        /// 重写方法 实体比较 !=
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(IntEntity a, IntEntity b)
        {
            return !(a == b);
        }
        /// <summary>
        /// 获取哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        /// <summary>
        /// 输出领域对象的状态
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }


    }
}
