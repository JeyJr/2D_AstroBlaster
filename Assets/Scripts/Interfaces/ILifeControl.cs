﻿public interface ILifeControl
{
    public void SetLifeValue(float value);
    public void GainLife(float value);
    public void LooseLife(float damage);
    public float GetLifeValue();
}
