import React, { Component } from 'react'
import './groupView.css'

class GroupView extends Component {
  render() {
    return (
      <>
        <h2>{ this.props.groupInfo.name }</h2>
        <h4>Tutor:</h4>
        { this.props.groupInfo.tutor.name + " " + this.props.groupInfo.tutor.surname}
      </>
    )
  }
}

export default GroupView