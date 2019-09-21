import * as React from "react";
import {v4 as uuid} from "uuid";

interface RadioGroupProps {
  options: Map<string, string>
  name: string
  onChange: (value: string) => void
  className?: string
}

class RadioGroup<T extends RadioGroupProps> extends React.Component<T, {}> {
  render() {
    let options: any[] = []
    let groupId = uuid()
    this.props.options.forEach((name, option) => {
      options.push(
        <p>
          <label>
            <input value={option} name={groupId} type="radio"
                   onChange={event => this.props.onChange(event.target.value)}/>
            <span>{name}</span>
          </label>
        </p>
      )
    })
    return (
      <div className={"col " + (this.props.className || "")}>
        {options}
      </div>
    )
  }
}

export default RadioGroup

