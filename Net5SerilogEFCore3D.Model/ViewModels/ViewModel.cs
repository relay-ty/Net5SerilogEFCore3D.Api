using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Model.ViewModels
{
    public class ViewModel<TKey>
    {
        [Key]
        public TKey Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

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
        public bool? IsEnable { get; set; } = true;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset? CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>        
        public Guid? CreateUserId { get; set; }

        /// <summary> 
        /// 修改时间
        /// </summary>
        public DateTimeOffset? ModifyTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public Guid? ModifyUserId { get; set; }
    }
}
