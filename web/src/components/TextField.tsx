import * as React from "react"
import {v4 as uuid} from "uuid"

interface TextFieldProps {
  onChange: (value: string) => void
  name: string
  className?: string
}

class TextField<T extends TextFieldProps> extends React.Component<T, {}> {
  render() {
    let id = uuid()
    return (
        <div className={"col input-field " + (this.props.className || "")}>
          <input id={id} type="text" onChange={event => this.props.onChange(event.target.value)}/>
          <label htmlFor={id}>{this.props.name}</label>
        </div>
    )
  }
}

export default TextField
