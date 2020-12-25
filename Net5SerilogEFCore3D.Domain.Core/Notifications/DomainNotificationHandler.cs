using MediatR;
using Net5SerilogEFCore3D.Model.DomainCoreModels;
using Net5SerilogEFCore3D.Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Net5SerilogEFCore3D.Domain.Core.Notifications
{
    /// <summary>
    /// 领域通知处理程序，把所有的通知信息放到事件总线中
    /// 继承 INotificationHandler<T>
    /// </summary>
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>, IDisposable
    {
        // 通知信息列表
        private List<DomainNotification> _ListNotification;

        // 每次访问该处理程序的时候，实例化一个空集合
        public DomainNotificationHandler()
        {
            _ListNotification = new List<DomainNotification>();
        }

        // 处理方法，把全部的通知信息，添加到内存里
        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            _ListNotification.Add(message);
            return Task.CompletedTask;
        }

        // 获取当前生命周期内的全部通知信息
        public virtual List<DomainNotification> GetNotifications()
        {
            return _ListNotification.OrderBy(o => o.Timestamp).ToList();
        }

        // 判断在当前总线对象周期中，是否存在通知信息
        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }
        // 判断在当前总线对象周期中，是否存在 Error 通知信息
        public virtual bool HasErrorNotifications()
        {
            return GetNotifications().Any(w => w.DomainNotificationType == DomainNotificationType.Error);
        }

        // 判断在当前总线对象周期中，是否存在 Success 通知信息
        public virtual bool HasSuccessNotifications()
        {
            return GetNotifications().Any(w => w.DomainNotificationType == DomainNotificationType.Error);
        }

        // 获取当前生命周期内的 DomainNotificationType 通知信息
        public virtual List<DomainNotification> GetSpecifyTypeNotifications(DomainNotificationType domainNotificationType)
        {
            return _ListNotification.Where(w => w.DomainNotificationType == domainNotificationType).ToList();
        }

        /// <summary>
        /// 处理消息返回
        /// </summary>
        /// <returns></returns>
        public MessageModel<object> ReturnNotifications(object data = null)
        {
            // 消息通知
            var notifications = GetNotifications();
            if (HasErrorNotifications())
            {
                return new MessageModel<object>()
                {
                    Success = false,
                    Message = string.Join(";", notifications.Select(s => s.Value)),
                    Data = data,
                    Notifications = notifications
                };
            }
            if (HasSuccessNotifications())
            {
                return new MessageModel<object>()
                {
                    Success = true,
                    Message = string.Join(";", notifications.Select(s => s.Value)),
                    Data = data,
                    Notifications = notifications
                };
            }
            return new MessageModel<object>()
            {
                Success = false,
                Message = string.Join(";", notifications.Select(s => s.Value)),
                Data = data,
                Notifications = notifications
            };
        }


        // 手动回收（清空通知）
        public void Dispose()
        {
            _ListNotification = new List<DomainNotification>();
            GC.SuppressFinalize(this);
        }
    }
}
