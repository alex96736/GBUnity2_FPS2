using System;

/// <summary>
/// Класс, описывающий таймер
/// </summary>
public sealed class Timer
{
    // время старта
    DateTime _start;
    // переменная для задания необходимой задержки 
    float _elapsed = -1;
    // текущее время
    TimeSpan _duration;

    public void Start(float elapsed)
    {
        elapsed = _elapsed;
        _start = DateTime.Now;
        _duration = TimeSpan.Zero;
    }

    public void Update()
    {
        if (_elapsed > 0)
        {
            _duration = DateTime.Now - _start;
            if (_duration.TotalSeconds > _elapsed)
            {
                _elapsed = 0;
            }

            if (_elapsed == 0)
            {
                _elapsed = -1;
            }
        }
    }

    /// <summary>
    /// Проврека таймера (сработал или нет)
    /// </summary>
    public bool IsEvent()
    {
        return _elapsed == 0;
    }
}
