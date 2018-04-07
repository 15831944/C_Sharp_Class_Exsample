#include "stdafx.h"
#include "AutoColor.h"
#include "stdafx.h"

void Edit(int * buf, int arrLen)
{
	//获取直方图
	vector<int*> Histogram(3, new int[256]);
	for (int i = 0; i < arrLen; i += 3)
	{
		Histogram[0][buf[i]]++;
		Histogram[1][buf[i + 1]]++;
		Histogram[2][buf[i + 2]]++;
	}

	//获取截断区
	int minmax[6], filter = arrLen / (6000 * 3);
	for (int i = 0; i < 3; i++)
	{
		int* hist = Histogram[i];
		int minV = 0, maxV = 255;

		for (; minV < 256; minV++)
			if (hist[minV] > filter && hist[minV] < hist[minV + 1] && hist[minV + 1] < hist[minV + 2])
				break;

		for (; maxV > -1; maxV--)
			if (hist[maxV] > filter && hist[maxV] < hist[maxV - 1] && hist[maxV - 1] < hist[maxV - 2])
				break;

		minmax[i * 2] = minV; minmax[i * 2 + 1] = maxV;

		//保护机制，触发后不对源数据做任何处理，保护域10/245
		//截断区与启始区过近，表示原图质量OK，不需要做处理
		//截断区过大，保留区不足以用于拉伸，表示原图质量过差，没有处理的意义
		//屏蔽已经做过拉伸的图像
		if ((minV < 10 && maxV > 245) || minV > 245 || maxV < 10) return;
	}

	//分波段拉伸
	for (int i = 0; i < 3; i++)
	{
		float zo = 255.0 / (minmax[i * 2 + 1] - minmax[i * 2]);
		for (int u = i; u < arrLen; u += 3)
		{
			int res = (buf[u] - minmax[i * 2]) * zo;//这里需要类型转换
			buf[u] = res < 0 ? 0 : res > 255 ? 255 : res;
		}
	}

	//gamma
	float gammas[3];
	{
		for (int i = 0; i < arrLen; i += 3)
		{
			gammas[0] += buf[i];
			gammas[1] += buf[i + 1];
			gammas[2] += buf[i + 2];
		}

		for (int i = 0; i < 3; i++)
			gammas[i] = (gammas[i] / (arrLen / 3)) / 100;

		for (int i = 0; i < arrLen; i++)
			buf[i] = pow(buf[i] / 255.0, gammas[i % 3]) * 255;//这里需要类型转换
	}
}
