﻿// Author: Daniele Giardini - http://www.demigiant.com
// Created: 2014/05/06 19:35
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using DG.Tween.Core;
using DG.Tween.Plugins.Core;
using UnityEngine;

namespace DG.Tween.Plugins.DefaultPlugins
{
    public class Vector3Plugin : ABSTweenPlugin<Vector3,Vector3,PlugVector3.Options>
    {
        public override Vector3 ConvertT1toT2(PlugVector3.Options options, Vector3 value)
        {
            return value;
        }

        public override Vector3 GetRelativeEndValue(PlugVector3.Options options, Vector3 startValue, Vector3 changeValue)
        {
            return startValue + changeValue;
        }

        public override Vector3 GetChangeValue(PlugVector3.Options options, Vector3 startValue, Vector3 endValue)
        {
            return endValue - startValue;
        }

        public override Vector3 Calculate(PlugVector3.Options options, MemberGetter<Vector3> getter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, EaseFunction ease)
        {
            startValue.x = ease(elapsed, startValue.x, changeValue.x, duration, 0, 0);
            startValue.y = ease(elapsed, startValue.y, changeValue.y, duration, 0, 0);
            startValue.z = ease(elapsed, startValue.z, changeValue.z, duration, 0, 0);
            if (options.snapping) {
                startValue.x = Mathf.Round(startValue.x);
                startValue.y = Mathf.Round(startValue.y);
                startValue.z = Mathf.Round(startValue.z);
            }
            return startValue;
        }
    }
}