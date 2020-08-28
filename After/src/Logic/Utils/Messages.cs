using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using Logic.AppServices;
using Logic.Dtos;

namespace Logic.Students
{
    public sealed class Messages
    {
        private readonly IServiceProvider _provider;

        public Messages(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Result Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(handlerType);
            Result result = handler.Handle((dynamic)command);

            return result;
        }

        public T Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs); // make somthing like this  IQueryHandler<GetListQuery, List<StudentDto>>

            // pool it from ioc , registerd alread by AddHandlers on startup
            dynamic handler = _provider.GetService(handlerType);
            //((IQueryHandler<GetListQuery, List<StudentDto>>)handler).Handle(null);

            T result = handler.Handle((dynamic)query);

            return result;
        }
    }
}
