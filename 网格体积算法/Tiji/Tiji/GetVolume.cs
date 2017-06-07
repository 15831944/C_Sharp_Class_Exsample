using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiji
{
    class GetVolume
    {
        List<PointD> points;
        List<int> indexs;
        double volumeRes;
        double minZ;
        public GetVolume(List<PointD> points, List<int> indexs)
        {
            this.points = points;
            this.indexs = indexs;
            volumeRes = 0;
            minZ = double.MaxValue;
            ee();
        }

        void ee()
        {
            //获取最低平面
            for (int i = 0; i < points.Count; i++)
            {
                minZ = points[i].z < minZ ? points[i].z : minZ;
            }
            //获取一个三角面
            for (int t = 0; t < indexs.Count; t += 3)
            {
                List<PointD> Triangle = new List<PointD>() { points[t], points[t + 1], points[t + 2] };
                //按Z值升序排列
                List<PointD> Triangle1 = Triangle.OrderBy(itm => itm.z).ToList();
                //最高点
                PointD A = Triangle1[2];
                //中间点
                PointD B = Triangle1[1];
                //最低点
                PointD C = Triangle1[0];
                //底平面映射点
                PointD D = new PointD(A.x, A.y, C.z);
                PointD E = new PointD(B.x, B.y, C.z);

                //三角形投影面积
                double S = (1 / 2) * (A.x * B.y + B.x * C.y + C.x * A.y - A.x * C.y - B.x * A.y - C.x * B.y);
                //三角柱体积
                volumeRes += S * (C.z - minZ);
                //五面体切分，四面体（1）
                volumeRes += _RedayToTetrahedronVolume(A, B, C, D);
                //五面体切分，四面体（2）
                volumeRes += _RedayToTetrahedronVolume(B, E, C, D);
            }
        }
        /// <summary>
        /// 通过四点获得四面体体积
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="t3"></param>
        /// <param name="t4"></param>
        /// <returns></returns>
        double _RedayToTetrahedronVolume(PointD t1, PointD t2, PointD t3, PointD t4)
        {
            return tetrahedronVolume(
                distance(t1, t2),
                distance(t1, t3),
                distance(t1, t4),
                distance(t3, t4),
                distance(t2, t4),
                distance(t2, t3)
                );
        }
        /// <summary>
        /// 获取两点间距的平方
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        double distance(PointD a, PointD b)
        {
            return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
        }
        /// <summary>
        /// 四面体体积
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        /// 记同一顶点引出的三条棱棱长的平方分别为a，b，c，它们的对棱棱长的平方分别为d，e，f，
        /// sqrt[ad(b + c + e + f - a - d) + be(a + c + d + f - b - e) + cf(a + b + d + e - c - f) - abf - bcd - cae - def)]/ 12
        /// ①直角四面体（三条侧棱两两互相垂直，记其长分别为a,b,c）：V=abc/6
        /// ②正四面体：棱长为a，则V=a^3*sqrt(2)/12
        /// ③等腰四面体（三组对棱都相等，记每组对棱的长分别为a,b,c，p=(a^2+b^2+c^2)/2）V=sqrt[(p - a)(p - b)(p - c)]
        double tetrahedronVolume(double a, double b, double c, double d, double e, double f)
        {
            double M01 = a * d * (b + c + e + f - a - d);
            double M02 = b * e * (a + c + d + f - b - e);
            double M03 = c * f * (a + b + d + e - c - f);
            double M04 = a * b * f - b * c * d - c * a * e - d * e * f;
            double res = Math.Sqrt(M01 + M02 + M03 - M04) / 12;
            return res;
        }
        /// <summary>
        /// 已知3点坐标，求平面ax+by+cz+d=0;，向量 (a,b,c) 就是此平面法线的法向量。        
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name=""></param>
        void get_panel(PointD p1, PointD p2, PointD p3)
        {
            double a = ((p2.y - p1.y) * (p3.z - p1.z) - (p2.z - p1.z) * (p3.y - p1.y));
            double b = ((p2.z - p1.z) * (p3.x - p1.x) - (p2.x - p1.x) * (p3.z - p1.z));
            double c = ((p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x));
            double d = (0 - (a * p1.x + b * p1.y + c * p1.z));

        }
        /// <summary>
        /// 已知三点坐标，求法向量,
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        PointD get_Normal(PointD p1, PointD p2, PointD p3)
        {
            double a = ((p2.y - p1.y) * (p3.z - p1.z) - (p2.z - p1.z) * (p3.y - p1.y));
            double b = ((p2.z - p1.z) * (p3.x - p1.x) - (p2.x - p1.x) * (p3.z - p1.z));
            double c = ((p2.x - p1.x) * (p3.y - p1.y) - (p2.y - p1.y) * (p3.x - p1.x));
            return new PointD(a, b, c);
        }
        /// <summary>
        /// 点到平面距离 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        double dis_pt2panel(PointD pt, double a, double b, double c, double d)
        {
            return Math.Abs(a * pt.x + b * pt.y + c * pt.z + d) / Math.Sqrt(a * a + b * b + c * c);
        }
        /// <summary>
        /// 求一条直线与平面的交点
        /// </summary>
        /// <param name="planeVector">平面的法线向量，长度为3</param>
        /// <param name="planePoint">平面经过的一点坐标，长度为3</param>
        /// <param name="lineVector">直线的方向向量，长度为3</param>
        /// <param name="linePoint">直线经过的一点坐标，长度为3</param>
        /// <returns>返回交点坐标，长度为3</returns>
        float[] CalPlaneLineIntersectPoint(float[] planeVector, float[] planePoint, float[] lineVector, float[] linePoint)
        {
            float[] returnResult = new float[3];
            float vp1, vp2, vp3, n1, n2, n3, v1, v2, v3, m1, m2, m3, t, vpt;
            vp1 = planeVector[0];
            vp2 = planeVector[1];
            vp3 = planeVector[2];
            n1 = planePoint[0];
            n2 = planePoint[1];
            n3 = planePoint[2];
            v1 = lineVector[0];
            v2 = lineVector[1];
            v3 = lineVector[2];
            m1 = linePoint[0];
            m2 = linePoint[1];
            m3 = linePoint[2];
            vpt = v1 * vp1 + v2 * vp2 + v3 * vp3;
            //首先判断直线是否与平面平行
            if (vpt == 0)
            {
                returnResult = null;
            }
            else
            {
                t = ((n1 - m1) * vp1 + (n2 - m2) * vp2 + (n3 - m3) * vp3) / vpt;
                returnResult[0] = m1 + v1 * t;
                returnResult[1] = m2 + v2 * t;
                returnResult[2] = m3 + v3 * t;
            }
            return returnResult;
        }
    }
    class PointD
    {
        public PointD(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public double x;
        public double y;
        public double z;
    }
}