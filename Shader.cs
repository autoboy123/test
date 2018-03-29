using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsyVBO
{
    public class ShaderHelper
    {
        /// <summary>
        /// 顶点着色器代码
        /// </summary>
        public static string VertexShaderSource =
         "#version 330 compatibility\n" +
            //"uniform vec4 offset;\n" +
            //"uniform mat4 projectionMatriax;\n" +
            //"uniform mat4 modelViewMatriax;\n" +
           // "layout(location=0) in vec3 position;\n" +
           // "layout(location=1) in vec3 color;\n" +
          //  "layout(location=2) in vec3 normal;\n" +
           // "out vec4 colorOut;\n" +
            "void main()\n" +
            "{\n" +
            "gl_Position = vec4(gl_Normal,1);"+
            "vec3 pos = gl_NormalMatrix * gl_Normal;"+
           //     "vec4 pos = vec4(position,1) - offset;" +
           //     "gl_Position = projectionMatriax * modelViewMatriax * pos;" +
           //     "colorOut = vec4(color,1);" +
           "vec3 d = vec3(1,0,0)*vec3(2,1,1);"+
            "}\n";
            //"#version 430 core\n" +
            //"uniform vec4 offset;\n" +
            //"uniform mat4 projectionMatriax;\n" +
            //"uniform mat4 modelViewMatriax;\n" +
            //"layout(location=0) in vec3 position;\n" +
            //"layout(location=1) in vec3 color;\n" +
            //"layout(location=2) in vec3 normal;\n" +
            //"out vec4 colorOut;\n" +
            //"void main()\n" +
            //"{\n" +
            //    "vec4 pos = vec4(position,1) - offset;" +
            //    "gl_Position = projectionMatriax * modelViewMatriax * pos;" +
            //    "colorOut = vec4(color,1);" +
            //"}\n";
        /// <summary>
        /// 片段着色器代码
        /// </summary>
        public static string FragmentShaderSource =
             "#version 330 compatibility\n" +
          //   "in vec4 colorOut;\n" +
         //    "out vec4 color1;\n" +
             "void main()\n" +
             "{" +
          //     "color1 = colorOut;" +
             "}\n";

        /// <summary>
        /// 片段着色器代码
        /// </summary>
        public static string GeometryShaderSource =
             "#version 430 core\n" +
            "layout(triangles) in;" +
            "layout(triangle_strip, max_vertices = 3) out;" +
             "void main()\n" +
             "{" +
                "gl_Position = gl_in[0].gl_Position;" +
                "gl_PointSize = gl_in[0].gl_PointSize;" +
                "EmitVertex();" +
                "EndPrimitive();" +
             "}\n";
    }

    public class ShaderBuilder
    {
        private uint program = 0;
        private uint vertexShader = 0;
        private uint fragmentShader = 0;
        public ShaderBuilder()
        {

        }
        public uint BuildProgram(OpenGL gl, string vertexShaderSource = null, string fragmentShaderSource = null)
        {
            if (vertexShaderSource == null && fragmentShaderSource == null)
            {
                vertexShader = gl.CreateShader(OpenGL.GL_VERTEX_SHADER);
                gl.ShaderSource(vertexShader, ShaderHelper.VertexShaderSource);
                gl.CompileShader(vertexShader);
                string compileInfo = GetShaderCompilorStatus(gl, vertexShader);
               
                //uint geoShader = gl.CreateShader(OpenGL.GL_GEOMETRY_SHADER);//创建几何着色器
                //gl.ShaderSource(geoShader, ShaderHelper.GeometryShaderSource);
                //gl.CompileShader(geoShader);
                //GetShaderCompilorStatus(gl, geoShader);

                fragmentShader = gl.CreateShader(OpenGL.GL_FRAGMENT_SHADER);
                gl.ShaderSource(fragmentShader, ShaderHelper.FragmentShaderSource);
                gl.CompileShader(fragmentShader);

                GetShaderCompilorStatus(gl, fragmentShader);

                program = gl.CreateProgram();
                gl.AttachShader(program, vertexShader);
                gl.AttachShader(program, fragmentShader);
                gl.LinkProgram(program);
                GetProgramLinkedInfoLog(gl, program);
                gl.ValidateProgram(program);
                //gl.UseProgram(program);
            }
            return program;
        }
        /// <summary>
        /// 查看GLSL程序着色器编译信息
        /// </summary>
        /// <param name="gl"></param>
        /// <param name="shader">片段着色器或顶点着色器</param>
        /// <returns></returns>
        private string GetShaderCompilorStatus(OpenGL gl, uint shader)
        {
            int[] status = { -1 };
            gl.GetShader(shader, OpenGL.GL_COMPILE_STATUS, status);
            if (status[0] == 0)
            {
                StringBuilder sb = new StringBuilder(100);
                gl.GetShaderInfoLog(shader, 500, IntPtr.Zero, sb);
                Console.WriteLine(sb);
                return sb.ToString();
            }
            else
                return "";
        }
        /// <summary>
        /// 获取GLSL程序链接信息
        /// </summary>
        /// <param name="gl"></param>
        /// <param name="program"></param>
        /// <returns></returns>
        private string GetProgramLinkedInfoLog(OpenGL gl, uint program)
        {
            int[] status = { -1 };
            gl.GetProgram(program, OpenGL.GL_LINK_STATUS, status);
            if (status[0] == 0)
            {
                StringBuilder sb = new StringBuilder(100);
                gl.GetProgramInfoLog(program, 500, IntPtr.Zero, sb);
                Console.WriteLine(sb.ToString());
                return sb.ToString();
            }
            else
                return "";
        }
        public void Dispose(OpenGL gl)
        {
            if (program != 0)
            {
                gl.DeleteShader(vertexShader);
                gl.DeleteShader(fragmentShader);
                gl.DeleteProgram(program);
            }
        }
    }
}