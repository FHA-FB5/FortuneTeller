import * as React from "react"
import {CSSProperties} from "react"
import "./wave.css"

interface WaveProps {
  colorTop: string
  colorBottom: string
  rotate: boolean | false
  maxWaves: number
  count: number
}

interface WaveState {
  width: number
  height: number
  style: CSSProperties
  paths: Array<any>
}

class Wave<P extends WaveProps, S extends WaveState> extends React.Component<P, S> {
  constructor(props: P) {
    super(props)
    let state: WaveState = {
      style: {},
      paths: [],
      width: Math.pow(2, this.props.maxWaves + 1),
      height: 100
    }
    let cTop = this.props.colorTop
    let cBottom = this.props.colorBottom
    if (this.props.rotate) {
      state.style.transform = "rotateZ(180deg)"
      let tmp = cTop
      cTop = cBottom
      cBottom = tmp
    }
    state.style.backgroundColor = cTop
    for (let i = 0; i < this.props.count; i++) {
      let curve = ""
      let waves = Math.ceil(Math.random() * this.props.maxWaves)
      let stepWidth = state.width
      for (let i = 0; i < waves + 1; i++) {
        stepWidth = stepWidth / 2
      }
      let stepHeight = state.height * .2 + state.height * .8 * Math.random()
      for (let x = 0; x < state.width / 4; x++) {
        curve += `${stepWidth} ${-stepHeight} ${stepWidth * 2} 0 ${stepWidth} ${stepHeight} ${stepWidth * 2} 0 `
      }
      curve += curve
      state.paths.push(
        <path
          d={`M ${-state.width} ${state.height / 2} q ${curve} V ${state.height} H ${-state.width}`}
          style={{
            animationDelay: `-${3 * i}s`,
            animationDuration: `${4 + (Math.random() * 4) + (this.props.maxWaves - waves + 1)}s`,
            opacity: (i + 1) / this.props.count,
            fill: cBottom,
            animationDirection: this.props.rotate ? "reverse" : "normal"
          }}/>
      )
    }
    this.state = state as S
  }

  render() {
    return (
      <svg className="waves" viewBox={`0 0 ${this.state.width} ${this.state.height}`} style={this.state.style}
           preserveAspectRatio="none">
        <g className="parallax">
          {this.state.paths}
        </g>
      </svg>
    )
  }
}

export default Wave
