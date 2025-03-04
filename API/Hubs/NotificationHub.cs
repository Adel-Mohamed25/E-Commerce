using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    public class NotificationHub : Hub
    {

        /// <summary>
        /// Sends a message to all connected clients.
        /// </summary>
        /// <param name="methodName">The event name that the client listens for (e.g., "ReceiveNotification").</param>
        /// <param name="message">The message payload to send.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An asynchronous task.</returns>
        public Task SendToAllAsync(string methodName, object message, CancellationToken cancellationToken)
        {
            return Clients.All.SendAsync(methodName, message, cancellationToken);
        }

        /// <summary>
        /// Sends a message to all connected clients except those with the specified connection IDs.
        /// </summary>
        /// <param name="methodName">The event name that the client listens for.</param>
        /// <param name="message">The message payload to send.</param>
        /// <param name="excludedConnectionIds">A collection of connection IDs to exclude.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An asynchronous task.</returns>
        public Task SendToAllExceptAsync(string methodName, object message, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken)
        {
            return Clients.AllExcept(excludedConnectionIds).SendAsync(methodName, message, cancellationToken);
        }

        /// <summary>
        /// Sends a message to a specific group.
        /// </summary>
        /// <param name="methodName">The event name that the client listens for.</param>
        /// <param name="message">The message payload to send.</param>
        /// <param name="group">The target group name.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An asynchronous task.</returns>
        public Task SendToGroupAsync(string methodName, object message, string group, CancellationToken cancellationToken)
        {
            return Clients.Group(group).SendAsync(methodName, message, cancellationToken);
        }

        /// <summary>
        /// Sends a message to a specific group while excluding certain connections.
        /// </summary>
        /// <param name="methodName">The event name that the client listens for.</param>
        /// <param name="message">The message payload to send.</param>
        /// <param name="group">The target group name.</param>
        /// <param name="excludedConnectionIds">A collection of connection IDs to exclude from the group.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An asynchronous task.</returns>
        public Task SendToGroupExceptAsync(string methodName, object message, string group, IEnumerable<string> excludedConnectionIds, CancellationToken cancellationToken)
        {
            return Clients.GroupExcept(group, excludedConnectionIds).SendAsync(methodName, message, cancellationToken);
        }

        /// <summary>
        /// Sends a message to multiple groups.
        /// </summary>
        /// <param name="methodName">The event name that the client listens for.</param>
        /// <param name="message">The message payload to send.</param>
        /// <param name="groupNames">A collection of target group names.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An asynchronous task.</returns>
        public Task SendToGroupsAsync(string methodName, object message, IEnumerable<string> groupNames, CancellationToken cancellationToken)
        {
            return Clients.Groups(groupNames).SendAsync(methodName, message, cancellationToken);
        }

        /// <summary>
        /// Sends a message to a specific user.
        /// </summary>
        /// <param name="methodName">The event name that the client listens for.</param>
        /// <param name="message">The message payload to send.</param>
        /// <param name="userId">The user identifier for the target user.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An asynchronous task.</returns>
        public Task SendToUserAsync(string methodName, object message, string userId, CancellationToken cancellationToken)
        {
            return Clients.User(userId).SendAsync(methodName, message, cancellationToken);
        }

        /// <summary>
        /// Sends a message to multiple specific users.
        /// </summary>
        /// <param name="methodName">The event name that the client listens for.</param>
        /// <param name="message">The message payload to send.</param>
        /// <param name="userIds">A collection of user identifiers for the target users.</param>
        /// <param name="cancellationToken">A token to cancel the operation if needed.</param>
        /// <returns>An asynchronous task.</returns>
        public Task SendToUsersAsync(string methodName, object message, IEnumerable<string> userIds, CancellationToken cancellationToken)
        {
            return Clients.Users(userIds).SendAsync(methodName, message, cancellationToken);
        }

    }
}
