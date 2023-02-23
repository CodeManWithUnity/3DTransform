using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _3DTransform
{
    public partial class Form1 : Form
    {
        int a;
        Triangle3D t;
        #region 模型变换
        //缩放矩阵
        Matrix4x4 m_scale;
        //旋转矩阵
        Matrix4x4 m_rotation;
        #endregion
        #region 摄像机变换
        //摄像机矩阵
        Matrix4x4 m_view;
        #endregion
        #region 投影变换
        //投影矩阵
        Matrix4x4 m_projection;
        #endregion
        public Form1()
        { 
            InitializeComponent();
            m_scale = new Matrix4x4();
            m_rotation = new Matrix4x4();
            m_scale[1, 1] = 250;
            m_scale[2, 2] = 250;
            m_scale[3, 3] = 250;
            m_scale[4, 4] = 1;
            m_view = new Matrix4x4();
            m_projection = new Matrix4x4();
            m_view[1, 1] = 1;
            m_view[2, 2] = 1;
            m_view[3, 3] = 1;
            m_view[4, 3] = 250;
            m_view[4, 4] = 1;
            m_projection[1, 1] = 1;
            m_projection[2, 2] = 1;
            m_projection[3, 3] = 1;
            m_projection[3, 4] = 1.0/250;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Vector4 a = new Vector4(0, -0.5, 0, 1);
            Vector4 b = new Vector4(0.5, 0.5, 0, 1);
            Vector4 c = new Vector4(-0.5, 0.5, 0, 1);
            t = new Triangle3D(a, b, c);
            t.Transform(m_scale);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            t.Draw(e.Graphics);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            a += 2;
            double angle = a / 360.0f * Math.PI;
            //以绕y轴转为例
            m_rotation[1, 1] = Math.Cos(angle);
            m_rotation[1, 3] = Math.Sin(angle);
            m_rotation[2, 2] = 1;
            m_rotation[3, 1] = -Math.Sin(angle);
            m_rotation[3, 3] = Math.Cos(angle);
            m_rotation[4, 4] = 1;

            Matrix4x4 m = m_scale.Mul(m_rotation);
            m = m.Mul(m_view);
            m = m.Mul(m_projection);
            //这里既旋转又缩放，这里是模型到世界的变换
            t.Transform(m);
            Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            m_view[4, 3] = (sender as TrackBar).Value;
        }
    }
}
