using mediatr
using System;
public interface ICommand<out TResponse> : IRequest<TResponse>
{

}
