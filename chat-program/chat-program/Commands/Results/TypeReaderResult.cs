using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatProgram.Commands
{
    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public struct TypeReaderValue
    {
        public object Value { get; }
        public float Score { get; }

        public TypeReaderValue(object value, float score)
        {
            Value = value;
            Score = score;
        }

        public override string ToString() => Value?.ToString();
        private string DebuggerDisplay => $"[{Value}, {Math.Round(Score, 2)}]";
    }

    [DebuggerDisplay(@"{DebuggerDisplay,nq}")]
    public struct TypeReaderResult : IResult
    {
        public List<TypeReaderValue> Values { get; }
        public CommandError? Error { get; }
        public string ErrorReason { get; }
        public bool IsSuccess => !Error.HasValue;

        /// <exception cref="InvalidOperationException">TypeReaderResult was not successful.</exception>
        public object BestMatch => IsSuccess
            ? (Values.Count == 1 ? Values.Single().Value : Values.OrderByDescending(v => v.Score).First().Value)
            : throw new InvalidOperationException("TypeReaderResult was not successful.");

        private TypeReaderResult(List<TypeReaderValue> values, CommandError? error, string errorReason)
        {
            Values = values;
            Error = error;
            ErrorReason = errorReason;
        }

        public static TypeReaderResult FromSuccess(object value)
            => new TypeReaderResult(new List<TypeReaderValue>() { new TypeReaderValue(value, 1.0f) }, null, null);
        public static TypeReaderResult FromSuccess(TypeReaderValue value)
            => new TypeReaderResult(new List<TypeReaderValue>() { value }, null, null);
        public static TypeReaderResult FromSuccess(List<TypeReaderValue> values)
            => new TypeReaderResult(values, null, null);
        public static TypeReaderResult FromError(CommandError error, string reason)
            => new TypeReaderResult(null, error, reason);
        public static TypeReaderResult FromError(Exception ex)
            => FromError(CommandError.Exception, ex.Message);
        public static TypeReaderResult FromError(IResult result)
            => new TypeReaderResult(null, result.Error, result.ErrorReason);

        public override string ToString() => IsSuccess ? "Success" : $"{Error}: {ErrorReason}";
        private string DebuggerDisplay => IsSuccess ? $"Success ({string.Join(", ", Values)})" : $"{Error}: {ErrorReason}";
    }
}
