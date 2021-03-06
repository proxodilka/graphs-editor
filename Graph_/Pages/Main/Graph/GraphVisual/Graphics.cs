﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Graph_.Canvas;

namespace Graph_.GraphVisual_
{
    public delegate void eventListener(object sender);
    public partial class GraphVisual
    {
        Graph graph;
        WFCanvas plot;
        Colors colors;
        int maxY, maxX;
        bool isDirected, isWeighted;
        float nodeSize = 1;

        public bool IsDirected { get { return isDirected; } set { isDirected = value; render(); } }
        public bool IsWeighted { get { return isWeighted; } set { isWeighted = value; render(); } }
        HashSet<int> activeVertices;
        Dictionary<int, Color> markedVertices;
        Dictionary<int, Node> nodes;
        Dictionary<int, Line> bindedLines;
        List<Tuple<int, int>> markedEdges;
        MutablePoint mouseCoords;
        bool animationStoped = false, hasActiveVertex=false, hasBindedLines=false, isShowingPath=false;
        public bool isCanvasDirty { get; internal set; }
        public event eventListener didUpdate;

        bool isInited;

        public GraphVisual(WFCanvas _plot, Graph _graph)
        {
            plot = _plot;
            graph = _graph;
            graph.edgeModified += onEdgesChanged;
            graph.vertexAdded += onVertexAdded;
            graph.vertexRemoved += onVertexRemoved;
            graph.rewrite += onRewrite;
            graph.isDirectedChanged += (object sender, GraphEventArgs e) => { IsDirected = (sender as Graph).IsDirected; };
            isInited = false;
            init();
        }

        public void init()
        {
            isCanvasDirty = false;
            animationStoped = false;
            hasActiveVertex = false;
            isShowingPath = false;


            activeVertices = new HashSet<int>();
            markedVertices = new Dictionary<int, Color>();
            colors = new Colors();
            nodes = new Dictionary<int, Node>();
            mouseCoords = new MutablePoint(new PointF(0, 0));
            bindedLines = new Dictionary<int, Line>();
            markedEdges = new List<Tuple<int, int>>();
            plot.onMouseMove += onMouseMove;

            onRewrite(nodes, new GraphEventArgs(graph.get().Keys.ToArray(), new int[0]));
            render(true);
            isInited = true;
        }

        private void render(bool isResetCenter=false)
        {
            plot.reset();
            drawVertices();
            drawEdges();
            drawBindedLines();
            
            if (isResetCenter)
            {
                //plot.setScale(calcScale());
                plot.setCenter(getPreferedCenter());
                plot.setScaleWithCorrection(calcScale());
            }

            plot.render();
            didUpdate?.Invoke(this);
        }

        public void reset(bool withoutRender=false)
        {
            markedVertices.Clear();
            activeVertices.Clear();
            markedEdges.Clear();
            isShowingPath = false;
            isCanvasDirty = false;
            animationStoped = false;
            hasActiveVertex = false;
            if (!withoutRender) render();
        }

        public void centrate()
        {
            render(true);
        }

    }
}
